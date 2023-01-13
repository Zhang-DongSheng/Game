using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.Window
{
    public partial class Artist : CustomWindow
    {
        private readonly Index indexPrefab = new Index();

        private List<ItemFile> list;

        private string[] _list;

        private ItemFile file;

        private GameObject prefab;

        private readonly List<Object> dependencies = new List<Object>();

        private readonly Color[] color = new Color[3] { Color.white, Color.white, Color.white };

        private void InitialisePrefab()
        {
            list = Finder.LoadFiles(Application.dataPath, "*.prefab");

            _list = new string[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                _list[i] = list[i].name;
            }

            indexPrefab.action = (index) =>
            {
                file = list[index];

                dependencies.Clear();

                if (string.IsNullOrEmpty(file.asset))
                {
                    prefab = null;
                }
                else
                {
                    prefab = AssetDatabase.LoadAssetAtPath<GameObject>(file.asset);

                    string[] _dependencies = AssetDatabase.GetDependencies(file.asset);

                    int count = _dependencies.Length;

                    for (int i = 0; i < count; i++)
                    {
                        var _dependency = AssetDatabase.LoadAssetAtPath<Object>(_dependencies[i]);

                        if (_dependency != prefab)
                        {
                            dependencies.Add(_dependency);
                        }
                    }
                }
            };
        }

        private void RefreshPrefab()
        {
            GUILayout.BeginHorizontal();
            {
                indexPrefab.value = EditorGUILayout.Popup(indexPrefab.value, _list);
            }
            GUILayout.EndHorizontal();

            GUILayout.Box(string.Empty, GUILayout.Height(3), GUILayout.ExpandWidth(true));

            if (file == null) return;

            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    EditorGUILayout.ObjectField(prefab, typeof(GameObject), false);

                    int count = dependencies.Count;

                    if (count > 0)
                    {
                        scroll = GUILayout.BeginScrollView(scroll);
                        {
                            GUI.enabled = false;

                            for (int i = 0; i < count; i++)
                            {
                                RefreshPrefabDependence(dependencies[i]);
                            }
                            GUI.enabled = true;
                        }
                        GUILayout.EndScrollView();
                    }
                }
                GUILayout.EndVertical();

                GUILayout.Box(string.Empty, GUILayout.Width(3), GUILayout.ExpandHeight(true));

                GUILayout.BeginVertical(GUILayout.Width(200));
                {
                    if (GUILayout.Button("检查组件引用"))
                    {
                        if (indexPrefab.value == 0) { }
                        else
                        {
                            Missing(file.asset);
                        }
                    }

                    GUILayout.BeginHorizontal();
                    {
                        color[0] = EditorGUILayout.ColorField(color[0]);

                        if (GUILayout.Button("图像"))
                        {
                            ModifyGraphicColor(file.asset, color[0]);
                        }
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        color[1] = EditorGUILayout.ColorField(color[1]);

                        if (GUILayout.Button("文本"))
                        {
                            ModifyTextColor(file.asset, color[1]);
                        }
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        color[2] = EditorGUILayout.ColorField(color[2]);



                        if (GUILayout.Button("阴影"))
                        {
                            ModifyShadowColor(file.asset, color[2]);
                        }
                    }
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("触发:false"))
                    {
                        ModifyGraphicRaycast(file.asset);
                    }

                    if (GUILayout.Button("复制"))
                    {
                        Selection.activeObject = prefab;

                        Open<PrefabCopy>("拷贝文件");
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshPrefabDependence(Object meta)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(meta.name, GUILayout.Width(100));

                EditorGUILayout.ObjectField(meta, typeof(Object), false);
            }
            GUILayout.EndHorizontal();
        }

        private void Missing(params string[] files)
        {
            foreach (var file in files)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(file);

                if (AssetDetection.Missing(prefab))
                {
                    Debug.LogError(string.Format("{0}丢失引用", prefab.name), prefab);
                }
                else
                {
                    Debug.LogFormat("{0}无丢失引用", prefab.name);
                }
            }
        }

        private void ModifyGraphicColor(string path, Color color)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            Graphic[] graphics = prefab.GetComponentsInChildren<Graphic>();

            for (int i = 0; i < graphics.Length; i++)
            {
                graphics[i].color = color;
            }
            PrefabUtility.SavePrefabAsset(prefab);
        }

        private void ModifyTextColor(string path, Color color)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            Text[] texts = prefab.GetComponentsInChildren<Text>();

            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].color = color;
            }
            PrefabUtility.SavePrefabAsset(prefab);
        }

        private void ModifyShadowColor(string path, Color color)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            Shadow[] shadows = prefab.GetComponentsInChildren<Shadow>();

            for (int i = 0; i < shadows.Length; i++)
            {
                shadows[i].effectColor = color;
            }
            PrefabUtility.SavePrefabAsset(prefab);
        }

        private void ModifyGraphicRaycast(string path)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            Graphic[] graphics = prefab.GetComponentsInChildren<Graphic>();

            for (int i = 0; i < graphics.Length; i++)
            {
                graphics[i].raycastTarget = false;
            }
            PrefabUtility.SavePrefabAsset(prefab);
        }
    }
}