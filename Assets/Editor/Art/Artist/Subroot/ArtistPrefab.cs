using Game;
using System;
using System.Collections.Generic;
using UnityEditor.Game;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UnityEditor.Window
{
    public class ArtistPrefab : ArtistBase
    {
        private readonly string[] options = new string[3] { "Component", "Sprite", "Text" };

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

        private readonly ComponentInformation component = new ComponentInformation();

        public override string Name => "Prefab";

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
            if (GUILayout.Button("Detection component references"))
            {
                if (indexPrefab.value == 0) { }
                else
                {
                    Missing(file.asset);
                }
            }

            GUILayout.BeginHorizontal();
            {
                component.from = EditorGUILayout.ObjectField(component.from, typeof(MonoScript), false) as MonoScript;

                GUILayout.Label(" > ", GUILayout.Width(25));

                component.to = EditorGUILayout.ObjectField(component.to, typeof(MonoScript), false) as MonoScript;
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("modify"))
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(file.asset);

                if (component.from != null && component.to != null && component.from != component.to)
                {
                    Type from = component.from.GetClass();

                    if (!from.IsInherit(typeof(Component)))
                    {
                        ShowNotification(string.Format("The from type of {0} is can't inherit Component!", component.from.name));
                        return;
                    }
                    else if (!component.to.GetClass().IsInherit(typeof(Component)))
                    {
                        ShowNotification(string.Format("The to type of {0} is can't inherit Component!", component.to.name));
                        return;
                    }
                    var components = prefab.GetComponentsInChildren(from);

                    for (int i = 0; i < components.Length; i++)
                    {
                        PrefabHelper.Replace(components[i], component.to);
                    }
                }
                PrefabUtility.SavePrefabAsset(prefab); indexPrefab.Execute();
            }
        }

        private void RefreshGraphic()
        {
            GUILayout.BeginVertical();
            {
                graphic.color = EditorGUILayout.ColorField(graphic.color);

                graphic.raycast = EditorGUILayout.Toggle("Trigger", graphic.raycast);

                if (GUILayout.Button("Modify"))
                {
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(file.asset);

                    Graphic[] graphics = prefab.GetComponentsInChildren<Graphic>();

                    for (int i = 0; i < graphics.Length; i++)
                    {
                        graphics[i].color = graphic.color;

                        graphics[i].raycastTarget = graphic.raycast;
                    }
                    PrefabUtility.SavePrefabAsset(prefab); indexPrefab.Execute();
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

                text.size = EditorGUILayout.IntField("Font size", text.size);

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
                if (GUILayout.Button("Modify"))
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
                                    texts[i].RemoveComponent<Outline>();

                                    var shadow = texts[i].AddOrReplaceComponent<Shadow>();

                                    shadow.effectColor = text.shadow;

                                    shadow.effectDistance = text.offset;
                                }
                                break;
                            case TextInformation.Extra.Outline:
                                {
                                    texts[i].RemoveComponent<Shadow>();

                                    var outline = texts[i].AddOrReplaceComponent<Outline>();

                                    outline.effectColor = text.shadow;

                                    outline.effectDistance = text.offset;
                                }
                                break;
                            case TextInformation.Extra.Clear:
                                {
                                    texts[i].RemoveComponent<Shadow>();

                                    texts[i].RemoveComponent<Outline>();
                                }
                                break;
                        }
                    }
                    PrefabUtility.SavePrefabAsset(prefab); indexPrefab.Execute();
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
                    Debug.LogError(string.Format("{0} Lost reference", prefab.name), prefab);
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
        [System.Serializable]
        class ComponentInformation
        {
            public MonoScript from;

            public MonoScript to;
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