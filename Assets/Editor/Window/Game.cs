using Data;
using Game.UI;
using System;
using System.IO;
using System.Reflection;
using UnityEditor.Game;
using UnityEngine;

namespace UnityEditor.Window
{
    public class Game : CustomWindow
    {
        private const string SCRIPT = "Scripts/UI/Panel/";

        private const string PREFAB = "Package/Prefab/UI/Panel/";

        private readonly string[] menu = new string[] { "Create", "Modify" };

        private UIPanel panel;

        private string content;

        private readonly UIInformation information = new UIInformation();
        [MenuItem("Game/UI")]
        protected static void Open()
        {
            Open<Game>("UI模板编辑");
        }

        protected override void Initialise() { }

        protected override void Refresh()
        {
            index.value = GUILayout.Toolbar(index.value, menu);

            switch (index.value)
            {
                case 0:
                    {
                        RefreshCreate();
                    }
                    break;
                case 1:
                    {
                        RefreshModify();
                    }
                    break;
            }
        }

        private void RefreshCreate()
        {
            content = GUILayout.TextField(content);

            if (GUILayout.Button(LanuageManager.Get("Create")))
            {
                if (string.IsNullOrEmpty(content)) return;

                string path = string.Format("Assets/{0}{1}.cs", SCRIPT, content);

                ScriptHandler.Create(path);

                AssetDatabase.ImportAsset(path);

                path = string.Format("Assets/{0}{1}.prefab", PREFAB, content);

                PrefabHandler.CreateUGUI(path);

                AssetDatabase.ImportAsset(path);

                ScriptHandler.Modify(typeof(UIPanel), content);

                AssetDatabase.Refresh();

                ShowNotification("模板创建完成");
            }

            if (GUILayout.Button(LanuageManager.Get("Reference")))
            {
                string path = string.Format("Assets/{0}{1}.prefab", PREFAB, content);

                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                Assembly assembly = Assembly.Load("Assembly-CSharp");

                Type type = assembly.GetType(string.Format("Game.UI.{0}", content));

                if (prefab != null && type != null)
                {
                    try
                    {
                        if (prefab.TryGetComponent(type, out _) == false)
                        {
                            prefab.AddComponent(type);
                        }
                        PrefabUtility.SavePrefabAsset(prefab);
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debuger.LogException(Author.Resource, e);
                    }
                }
                else
                {
                    UnityEngine.Debuger.LogError(Author.Resource, "Prefab or Script is null!");
                }
                AddOrReplaceUIInformation();
            }
        }

        private void RefreshModify()
        {
            panel = (UIPanel)EditorGUILayout.EnumPopup(panel);

            if (information.panel != panel)
            {
                var asset = AssetDatabase.LoadAssetAtPath<DataUI>("Assets/Package/Data/DataUI.asset");

                var info = asset.list.Find(x => x.panel == panel);

                if (info != null)
                {
                    information.Copy(info);
                }
                else
                {
                    information.panel = panel;
                }
            }
            RefreshUIInformation();

            if (GUILayout.Button(LanuageManager.Get("Modify")))
            {
                ModifyUIInformation();
            }
        }

        private void RefreshUIInformation()
        {
            float width = 100;

            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(LanuageManager.Get("Panel"), GUILayout.Width(width));

                    GUILayout.Label(information.panel.ToString());
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(LanuageManager.Get("Layer"), GUILayout.Width(width));

                    information.layer = (UILayer)EditorGUILayout.EnumPopup(information.layer);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(LanuageManager.Get("Type"), GUILayout.Width(width));

                    information.type = (UIType)EditorGUILayout.EnumPopup(information.type);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(LanuageManager.Get("Name"), GUILayout.Width(width));

                    information.name = EditorGUILayout.TextField(information.name);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(LanuageManager.Get("Record"), GUILayout.Width(width));

                    information.destroy = GUILayout.Toggle(information.destroy, string.Empty);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(LanuageManager.Get("Order"), GUILayout.Width(width));

                    information.order = EditorGUILayout.IntField(information.order);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(LanuageManager.Get("Path"), GUILayout.Width(width));

                    information.path = EditorGUILayout.TextField(information.path);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private void AddOrReplaceUIInformation()
        {
            var asset = AssetDatabase.LoadAssetAtPath<DataUI>("Assets/Package/Data/DataUI.asset");

            if (asset.list.Exists(x => x.panel.ToString() == content))
            {
                
            }
            else
            {
                var clone = new UIInformation();

                clone.Copy(information);

                clone.panel = Enum.Parse<UIPanel>(content);

                asset.list.Add(clone);

                AssetDatabase.SaveAssets();
            }
            AssetDatabase.Refresh();
        }

        private void ModifyUIInformation()
        {
            var asset = AssetDatabase.LoadAssetAtPath<DataUI>("Assets/Package/Data/DataUI.asset");

            int index = asset.list.FindIndex(x => x.panel == information.panel);

            if (index > -1) 
            {
                asset.list[index].Copy(information);

                AssetDatabase.SaveAssets();
            }
            AssetDatabase.Refresh();
        }
    }
}