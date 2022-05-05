using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.Hierarchy
{
    class Templet
    {
        [MenuItem("GameObject/UIExtension/Button/AlphaButton")]
        protected static void CreateAlphaButton()
        {
            GameObject go = CreateGameObject("AlphaButton", true);

            AlphaButton button = go.AddComponent<AlphaButton>();

            Selection.activeGameObject = go;
        }
        [MenuItem("GameObject/UIExtension/Button/ButtonPro")]
        protected static void CreateButtonPro()
        {
            GameObject go = CreateGameObject("ButtonPro", true);

            Image image = go.AddComponent<Image>();

            ButtonPro button = go.AddComponent<ButtonPro>();

            Selection.activeGameObject = go;
        }

        protected static GameObject CreateGameObject(string name, bool ugui = false)
        {
            GameObject go = new GameObject(name);

            go.transform.SetParent(Parent(ugui));

            go.transform.localRotation = Quaternion.identity;

            go.transform.localPosition = Vector3.zero;

            go.transform.localScale = Vector3.one;

            go.layer = ugui ? 5 : 0;

            if (ugui)
            {
                go.AddComponent<RectTransform>();
            }
            return go;
        }

        protected static Transform Parent(bool ugui = false)
        {
            if (ugui)
            {
                if (Selection.activeTransform is RectTransform)
                {
                    return Selection.activeTransform;
                }
                else
                {
                    Canvas canvas = GameObject.FindObjectOfType<Canvas>();

                    if (canvas == null)
                    {
                        canvas = new GameObject("Canvas").AddComponent<Canvas>();
                    }
                    return canvas.transform;
                }
            }
            return Selection.activeTransform;
        }
    }
}