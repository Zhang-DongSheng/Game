using Data;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace UnityEditor.Window
{
    public class ResourcesBuilder : CustomWindow
    {
        private List<Node> nodes;

        private DataResource data;

        [MenuItem("Data/Resources")]
        protected static void Open()
        {
            Open<ResourcesBuilder>("Resources工具");
        }

        private void UpdateAsset()
        {
            nodes = Finder.Find(Application.dataPath + "/Resources");

            for (int i = 0; i < nodes.Count; i++)
            {
                Select(nodes[i]);
            }
        }

        private void Load()
        {
            data = DataManager.Instance.Load<DataResource>("Resource", "Data/Resource");
        }

        private void Select(Node node)
        {
            if (node.type == NodeType.Folder)
            {
                NodeFolder folder = node as NodeFolder;

                for (int i = 0; i < folder.nodes.Count; i++)
                {
                    Select(folder.nodes[i]);
                }
            }
            else
            {
                NodeFile file = node as NodeFile;

                file.select = data.Exist(node.name);
            }
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

            Load();

            if (data == null)
            {
                DataBuilder.Create_Resource(); Load();
            }

            UpdateAsset();
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
                        for (int i = 0; i < nodes.Count; i++)
                        {
                            RefreshNode(nodes[i]);
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
                        UpdateAsset();
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

        private void RefreshNode(Node node)
        {
            switch (node.type)
            {
                case NodeType.Folder:
                    RefreshFolder(node as NodeFolder);
                    break;
                case NodeType.File:
                    RefreshFile(node as NodeFile);
                    break;
            }
        }

        private void RefreshFolder(NodeFolder node)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(20));
            {
                for (int i = 0; i < node.order; i++)
                {
                    GUILayout.Space(20);
                }
                if (GUILayout.Button(node.status ? "▲" : "▼", GUILayout.Width(30)))
                {
                    node.status = !node.status;
                }
                GUILayout.Label(string.Format("{0} >", Format(node.name)), style[0]);
            }
            GUILayout.EndHorizontal();

            if (node.status)
            {
                for (int i = 0; i < node.nodes.Count; i++)
                {
                    RefreshNode(node.nodes[i]);
                }
            }
        }

        private void RefreshFile(NodeFile node)
        {
            GUILayout.BeginHorizontal();
            {
                for (int i = 0; i < node.order; i++)
                {
                    GUILayout.Space(20);
                }
                GUILayout.Label(Format(node.path), style[1]);

                node.name = GUILayout.TextField(node.name, GUILayout.Width(100));

                node.select = GUILayout.Toggle(node.select, "", GUILayout.Width(20));
            }
            GUILayout.EndHorizontal();
        }

        private void Build()
        {
            data.resources.Clear();

            for (int i = 0; i < nodes.Count; i++)
            {
                Builder(nodes[i]);
            }
            EditorUtility.SetDirty(data);

            AssetDatabase.Refresh();
        }

        private void Builder(Node node)
        {
            if (node.type == NodeType.Folder)
            {
                NodeFolder folder = node as NodeFolder;

                for (int i = 0; i < folder.nodes.Count; i++)
                {
                    Builder(folder.nodes[i]);
                }
            }
            else
            {
                if (node is NodeFile file && file.select)
                {
                    data.resources.Add(new ResourceInformation()
                    {
                        key = file.name,
                        capacity = -1,
                        secret = Md5Tools.ComputeFile(file.path),
                        prefab = AssetDatabase.LoadAssetAtPath(Format(file.path), typeof(Object)),
                        description = file.path,
                    });
                }
            }
        }

        private string Format(string path)
        {
            return path.Remove(0, Application.dataPath.Length - 6).Replace("\\", "/");
        }
    }
}