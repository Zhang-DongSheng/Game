using Game.UI;
using UnityEngine;

namespace UnityEditor.Inspector
{
    [CustomEditor(typeof(HotfixComponents))]
    public class InspectorHotfixComponents : Editor
    {
        public override void OnInspectorGUI()
        {
            base.DrawDefaultInspector();

            if (GUILayout.Button("¸üÐÂ"))
            {
                GenerateHotfixComponents();
            }
        }

        private void GenerateHotfixComponents()
        {
            var target = this.target as HotfixComponents;

            var gameobject = target.gameObject;

            var name = gameobject.name[..^4];

            string path = string.Format("Assets/ILRuntime/Hotfix~/Script/UI/Hall/{0}/IL{0}Relevance.cs", name);

            ScriptUtils.CreateILRuntimeComponents(path, target);

            Debuger.LogNotifycation(Author.ILRuntime, $"IL{name}Relevance is Generated!");
        }
    }
    /// <summary>
    /// Custom property drawer for ILRuntimeComponent.
    /// </summary>
    [CustomPropertyDrawer(typeof(HotfixComponent))]
    class ILRuntimeComponentDrawer : PropertyDrawer
    {
        private readonly Rect[] rects = new Rect[4];

        private float width, height;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2 + 20;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                EditorGUIUtility.labelWidth = 50;

                width = position.width * 0.5f - 15;

                height = EditorGUIUtility.singleLineHeight;

                int count = rects.Length / 2;

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < count; j++)
                    {
                        var index = i * count + j;

                        rects[index] = new Rect(position)
                        {
                            x = position.x + j * width + 10,
                            y = position.y + i * height + i * 10,
                            width = width,
                            height = height,
                        };
                    }
                }
                EditorGUI.PropertyField(rects[0], property.FindPropertyRelative("key"));

                EditorGUI.PropertyField(rects[1], property.FindPropertyRelative("target"));

                EditorGUI.PropertyField(rects[2], property.FindPropertyRelative("type"));

                var type = property.FindPropertyRelative("type").enumValueIndex;

                if (type == (int)ILRuntimeComponentType.Custom)
                {
                    EditorGUI.PropertyField(rects[3], property.FindPropertyRelative("custom"));
                }
            }
        }
    }
}