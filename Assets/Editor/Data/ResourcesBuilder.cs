using Data;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Window
{
    public class ResourcesBuilder : CustomWindow
    {
        private DataResource data;

        private List<ItemBase> items;

        [MenuItem("Data/Resources")]
        protected static void Open()
        {
            Open<ResourcesBuilder>("Resources工具");
        }

        protected override void Init()
        {
            style.Clear();

            style.Add(new GUIStyle()
            {
                fontStyle = FontStyle.Bold,
                fontSize = 20,
            });
            style[0].normal.textColor = Color.blue;

            style.Add(new GUIStyle()
            {

            });
            style[1].normal.textColor = Color.red;

            data = DataManager.Instance.Load<DataResource>("Resource", "Data/Resource");

            if (data == null)
            {
                DataBuilder.Create_Resource();

                data = DataManager.Instance.Load<DataResource>("Resource", "Data/Resource");
            }

            UpdateResources();
        }

        protected override void Refresh()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Assets > Resources");
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Box(string.Empty, GUILayout.ExpandWidth(true), GUILayout.Height(3));

                    scroll = GUILayout.BeginScrollView(scroll);
                    {
                        for (int i = 0; i < items.Count; i++)
                        {
                            RefreshNode(items[i]);
                        }
                    }
                    GUILayout.EndScrollView();
                }
                GUILayout.EndVertical();

                GUILayout.Box(string.Empty, GUILayout.Width(3), GUILayout.ExpandHeight(true));

                GUILayout.BeginVertical(GUILayout.Width(100));
                {
                    GUILayout.Space(30);

                    if (GUILayout.Button("刷新", GUILayout.Height(30)))
                    {
                        UpdateResources();
                    }

                    if (GUILayout.Button("生成Asset", GUILayout.Height(20)))
                    {
                        Build();
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshNode(ItemBase item)
        {
            switch (item.type)
            {
                case ItemType.Folder:
                    RefreshFolder(item as ItemFolder);
                    break;
                case ItemType.File:
                    RefreshFile(item as ItemFile);
                    break;
            }
        }

        private void RefreshFolder(ItemFolder folder)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(20));
            {
                for (int i = 0; i < folder.order; i++)
                {
                    GUILayout.Space(20);
                }
                if (GUILayout.Button(folder.select ? "▲" : "▼", GUILayout.Width(30)))
                {
                    folder.select = !folder.select;
                }
                GUILayout.Label(string.Format("{0} >", folder.name), style[0]);
            }
            GUILayout.EndHorizontal();

            if (folder.select)
            {
                for (int i = 0; i < folder.items.Count; i++)
                {
                    RefreshNode(folder.items[i]);
                }
            }
        }

        private void RefreshFile(ItemFile file)
        {
            GUILayout.BeginHorizontal();
            {
                for (int i = 0; i < file.order; i++)
                {
                    GUILayout.Space(20);
                }
                GUILayout.Label(file.name, style[1]);

                file.name = GUILayout.TextField(file.name, GUILayout.Width(100));

                file.select = GUILayout.Toggle(file.select, "", GUILayout.Width(20));
            }
            GUILayout.EndHorizontal();
        }

        private void UpdateResources()
        {
            items = Finder.Find(Application.dataPath + "/Resources");

            for (int i = 0; i < items.Count; i++)
            {
                Select(items[i]);
            }
        }

        private void Select(ItemBase item)
        {
            if (item.type == ItemType.Folder)
            {
                ItemFolder folder = item as ItemFolder;

                for (int i = 0; i < folder.items.Count; i++)
                {
                    Select(folder.items[i]);
                }
            }
            else
            {
                item.select = data.Exist(item.name);
            }
        }

        private void Build()
        {
            data.resources.Clear();

            for (int i = 0; i < items.Count; i++)
            {
                Builder(items[i]);
            }
            EditorUtility.SetDirty(data);

            AssetDatabase.Refresh();
        }

        private void Builder(ItemBase item)
        {
            if (item.type == ItemType.Folder)
            {
                ItemFolder folder = item as ItemFolder;

                for (int i = 0; i < folder.items.Count; i++)
                {
                    Builder(folder.items[i]);
                }
            }
            else
            {
                if (item is ItemFile file && file.select)
                {
                    data.resources.Add(new ResourceInformation()
                    {
                        identification = AssetDatabase.LoadAssetAtPath(file.asset, typeof(Object)).GetInstanceID(),
                        key = file.name,
                        capacity = -1,
                        secret = Md5Tools.ComputeFile(file.path),
                        prefab = AssetDatabase.LoadAssetAtPath(file.asset, typeof(Object)),
                        description = file.asset,
                    });
                }
            }
        }
    }
}