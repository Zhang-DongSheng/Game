using Data;
using Game.UI;
using System;
using System.Reflection;
using UnityEngine;
using Utility = Game.Utility;

namespace UnityEditor.Window
{
    public class UIEditor : CustomWindow
    {
        private const float WIDTH = 100;

        private readonly string[] menu = new string[] { "Create", "Modify" };

        private string content;

        private bool relevance;

        private UIPanel panel;

        private UIInformation source;

        private UIInformation information = new UIInformation();
        [MenuItem("Game/UI Editor")]
        protected static void Open()
        {
            Open<UIEditor>("UI编辑");
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
            GUILayout.Label("Create New UI");

            content = GUILayout.TextField(content);

            if (GUILayout.Button(ToLanguage("Create")))
            {
                if (string.IsNullOrEmpty(content)) return;

                string path = string.Format("Assets/Scripts/UI/Hall/{0}/{0}View.cs", content);

                ScriptUtils.Create(path);

                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

                AssetDatabase.Refresh();

                path = string.Format("Assets/{0}/{1}View.prefab", UIDefine.Prefab, content);

                PrefabUtils.CreateUGUI(path);

                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

                ScriptUtils.ModifyEnum(typeof(UIPanel), content);

                AssetDatabase.Refresh();

                ShowNotification("模板创建完成");
            }
        }

        private void RefreshModify()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Panel", GUILayout.Width(WIDTH));

                panel = (UIPanel)EditorGUILayout.EnumPopup(panel);
            }
            GUILayout.EndHorizontal();

            if (panel == UIPanel.None) return;

            if (information.panel != (int)panel)
            {
                var asset = AssetDatabase.LoadAssetAtPath<DataUI>("Assets/Package/Data/DataUI.asset");

                source = asset.list.Find(x => x.panel == (int)panel);

                if (source != null)
                {
                    information = new UIInformation(source);
                }
                else
                {
                    information = new UIInformation()
                    {
                        panel = (int)panel,

                        type = UIType.Panel,

                        layer = UILayer.Window,

                        name = panel.ToString(),

                        destroy = false,

                        order = 0,

                        path = string.Format("{0}/{1}View.prefab", UIDefine.Prefab, panel)
                    };
                }
                relevance = false;
            }
            RefreshUIInformation();

            if (source != null)
            {
                if (Utility.Class.Compare<UIInformation>(source, information))
                {
                    // do nothing...
                }
                else
                {
                    if (GUILayout.Button(ToLanguage("Modify")))
                    {
                        AddOrReplaceUIInformation();
                    }
                    relevance = false;
                }
            }
            else
            {
                if (GUILayout.Button(ToLanguage("Create")))
                {
                    AddOrReplaceUIInformation();
                }
            }
            // 检查预制体与代码是否关联
            if (!relevance)
            {
                var asset = AssetDatabase.LoadAssetAtPath<GameObject>(string.Format("Assets/{0}", information.path));

                if (asset == null)
                {
                    EditorGUILayout.HelpBox("未发现预制体资源！", MessageType.Error);
                }
                else if (asset.TryGetComponent(out ViewBase view))
                {
                    relevance = true;
                }
            }

            if (!relevance)
            {
                if (GUILayout.Button(ToLanguage("Reference")))
                {
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(string.Format("Assets/{0}", information.path));

                    Assembly assembly = Assembly.Load("Assembly-CSharp");

                    Type type = assembly.GetType(string.Format("Game.UI.{0}", information.name));

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
                        UnityEngine.Debuger.LogError(Author.Resource, "prefab or script is null!");
                    }
                    AddOrReplaceUIInformation();
                }
            }
        }

        private void RefreshUIInformation()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Layer", GUILayout.Width(WIDTH));

                    information.layer = (UILayer)EditorGUILayout.EnumPopup(information.layer);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Type", GUILayout.Width(WIDTH));

                    information.type = (UIType)EditorGUILayout.EnumPopup(information.type);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Order", GUILayout.Width(WIDTH));

                    information.order = (uint)EditorGUILayout.IntField((int)information.order);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Destroy", GUILayout.Width(WIDTH));

                    information.destroy = GUILayout.Toggle(information.destroy, string.Empty);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Path", GUILayout.Width(WIDTH));

                    information.path = EditorGUILayout.TextField(information.path);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private void AddOrReplaceUIInformation()
        {
            var asset = AssetDatabase.LoadAssetAtPath<DataUI>("Assets/Package/Data/DataUI.asset");

            int index = asset.list.FindIndex(x => x.panel == information.panel);

            if (index > -1)
            {
                asset.list[index] = new UIInformation(information);
            }
            else
            {
                asset.list.Add(information);
            }
            AssetDatabase.SaveAssets();

            AssetDatabase.Refresh();
        }
    }
}