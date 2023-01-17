using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.Window
{
    public class ArtistPrefab : ArtistBase
    {
        private readonly string[] options = new string[3] { "组件", "图像", "文本" };

        private readonly Index indexPrefab = new Index();

        private readonly Index indexMenu = new Index();

        private List<ItemFile> list;

        private string[] _list;

        private ItemFile file;

        private GameObject prefab;

        private Vector2 scroll = new Vector2();

        private readonly List<Object> dependencies = new List<Object>();

        private readonly TextInformation text = new TextInformation();

        private readonly GraphicInformation graphic = new GraphicInformation();

        public override string Name => "预制体";

        public override void Initialise()
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

        public override void Refresh()
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
                                RefreshDependence(dependencies[i]);
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
                    indexMenu.value = GUILayout.Toolbar(indexMenu.value, options);

                    switch (indexMenu.value)
                    {
                        case 0:
                            {
                                RefreshComponent();
                            }
                            break;
                        case 1:
                            {
                                RefreshGraphic();
                            }
                            break;
                        case 2:
                            {
                                RefreshText();
                            }
                            break;
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshDependence(Object meta)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(meta.name, GUILayout.Width(100));

                EditorGUILayout.ObjectField(meta, typeof(Object), false);
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshComponent()
        {
            if (GUILayout.Button("检查组件引用"))
            {
                if (indexPrefab.value == 0) { }
                else
                {
                    Missing(file.asset);
                }
            }
        }

        private void RefreshGraphic()
        {
            GUILayout.BeginVertical();
            {
                graphic.color = EditorGUILayout.ColorField(graphic.color);

                graphic.raycast = EditorGUILayout.Toggle("触发", graphic.raycast);

                if (GUILayout.Button("修改"))
                {
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(file.asset);

                    Graphic[] graphics = prefab.GetComponentsInChildren<Graphic>();

                    for (int i = 0; i < graphics.Length; i++)
                    {
                        graphics[i].color = graphic.color;

                        graphics[i].raycastTarget = graphic.raycast;
                    }
                    PrefabUtility.SavePrefabAsset(prefab); indexPrefab.Excute();
                }
            }
            GUILayout.EndVertical();
        }

        private void RefreshText()
        {
            GUILayout.BeginVertical();
            {
                text.style = (FontStyle)EditorGUILayout.EnumPopup(text.style);

                text.color = EditorGUILayout.ColorField(text.color);

                text.size = EditorGUILayout.IntField("字号", text.size);

                text.extra = (TextInformation.Extra)EditorGUILayout.EnumPopup(text.extra);

                switch (text.extra)
                {
                    case TextInformation.Extra.None:
                    case TextInformation.Extra.Clear:
                        break;
                    default:
                        {
                            text.shadow = EditorGUILayout.ColorField(text.shadow);

                            text.offset = EditorGUILayout.Vector2Field(string.Empty, text.offset);
                        }
                        break;
                }
                if (GUILayout.Button("修改"))
                {
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(file.asset);

                    Text[] texts = prefab.GetComponentsInChildren<Text>();

                    for (int i = 0; i < texts.Length; i++)
                    {
                        texts[i].fontStyle = text.style;

                        texts[i].color = text.color;

                        texts[i].fontSize = text.size;

                        switch (text.extra)
                        {
                            case TextInformation.Extra.Shadow:
                                {
                                    RemoveComponent<Outline>(texts[i]);

                                    var shadow = AddComponent<Shadow>(texts[i]);

                                    shadow.effectColor = text.shadow;

                                    shadow.effectDistance = text.offset;
                                }
                                break;
                            case TextInformation.Extra.Outline:
                                {
                                    RemoveComponent<Shadow>(texts[i]);

                                    var outline = AddComponent<Outline>(texts[i]);

                                    outline.effectColor = text.shadow;

                                    outline.effectDistance = text.offset;
                                }
                                break;
                            case TextInformation.Extra.Clear:
                                {
                                    RemoveComponent<Shadow>(texts[i]);

                                    RemoveComponent<Outline>(texts[i]);
                                }
                                break;
                        }
                    }
                    PrefabUtility.SavePrefabAsset(prefab); indexPrefab.Excute();
                }
            }
            GUILayout.EndVertical();
        }

        private void Missing(params string[] files)
        {
            foreach (var file in files)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(file);

                if (Missing(prefab))
                {
                    Debug.LogError(string.Format("{0}丢失引用", prefab.name), prefab);
                }
                else
                {
                    Debug.LogFormat("{0}无丢失引用", prefab.name);
                }
            }
        }

        private bool Missing(GameObject go)
        {
            bool missing = false;

            Component[] components = go.GetComponentsInChildren<Component>(true);

            foreach (var component in components)
            {
                if (component != null)
                {
                    SerializedObject so = new SerializedObject(component);

                    SerializedProperty sp = so.GetIterator();

                    while (sp.NextVisible(true))
                    {
                        if (sp.propertyType == SerializedPropertyType.ObjectReference &&
                            sp.objectReferenceInstanceIDValue != 0 &&
                            sp.objectReferenceValue == null)
                        {
                            Debug.LogWarningFormat("{0} Missing : {1}", go.name, sp.propertyPath);
                        }
                    }
                }
                else
                {
                    missing = true;
                }
            }
            return missing;
        }

        private T AddComponent<T>(Component target) where T : Component
        {
            if (target == null) return null;

            if (target.TryGetComponent(out T component))
            {

            }
            else
            {
                component = target.gameObject.AddComponent<T>();
            }
            return component;
        }

        private void RemoveComponent<T>(Component target) where T : Component
        {
            if (target == null) return;

            if (target.TryGetComponent(out T component))
            {
                UnityEngine.Object.DestroyImmediate(component, true);
            }
        }
        [System.Serializable]
        class GraphicInformation
        {
            public Color color = new Color(1, 1, 1, 1);

            public bool raycast;
        }
        [System.Serializable]
        class TextInformation
        {
            public FontStyle style = FontStyle.Normal;

            public Color color = new Color(1, 1, 1, 1);

            public int size = 28;

            public Extra extra = Extra.None;

            public Color shadow = new Color(0, 0, 0, 0.5f);

            public Vector2 offset = new Vector2(1, -1);

            public enum Extra
            {
                None,
                Shadow,
                Outline,
                Clear,
            }
        }
    }
}