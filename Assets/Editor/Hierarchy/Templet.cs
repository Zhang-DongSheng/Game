using Game;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.Hierarchy
{
    class Templet
    {
        protected const string EXTENSION = "GameObject/UI/Extension/";

        protected const string TEMPLATE = "GameObject/UI/Templet/";

        #region UGUI Extension
        [MenuItem(EXTENSION + "Button/AlphaButton")]
        protected static void CreateAlphaButton()
        {
            GameObject go = CreateGameObject("AlphaButton", true);

            var component = go.AddComponent<AlphaButton>();
        }
        [MenuItem(EXTENSION + "Button/ButtonPro")]
        protected static void CreateButtonPro()
        {
            GameObject go = CreateGameObject("ButtonPro", true);

            Image image = go.AddComponent<Image>();

            image.sprite = GetSprite();

            var component = go.AddComponent<ButtonPro>();
        }
        [MenuItem(EXTENSION + "Button/CodeButton")]
        protected static void CreateCodeButton()
        {
            GameObject go = CreateGameObject("CodeButton", true);

            var component = go.AddComponent<CodeButton>();
        }
        [MenuItem(EXTENSION + "Button/EmptyButton")]
        protected static void CreateEmptyButton()
        {
            GameObject go = CreateGameObject("EmptyButton", true);

            var component = go.AddComponent<EmptyButton>();
        }
        [MenuItem(EXTENSION + "Button/PolygonImageButton")]
        protected static void CreatePolygonImageButton()
        {
            GameObject go = CreateGameObject("PolygonImageButton", true);

            var component = go.AddComponent<PolygonImageButton>();
        }
        [MenuItem(EXTENSION + "Button/PolygonSpriteButton")]
        protected static void CreatePolygonSpriteButton()
        {
            GameObject go = CreateGameObject("PolygonSpriteButton", true);

            var component = go.AddComponent<PolygonSpriteButton>();
        }
        [MenuItem(EXTENSION + "Graphic/Circle/Circle")]
        protected static void CreateCircleCircle()
        {
            GameObject go = CreateGameObject("Circle", true);

            var component = go.AddComponent<Circle>();
        }
        [MenuItem(EXTENSION + "Graphic/Circle/CirclePro")]
        protected static void CreateCircleCirclePro()
        {
            GameObject go = CreateGameObject("CirclePro", true);

            var component = go.AddComponent<CirclePro>();
        }
        [MenuItem(EXTENSION + "Graphic/Polygon")]
        protected static void CreatePolygon()
        {
            GameObject go = CreateGameObject("Polygon", true);

            var component = go.AddComponent<Polygon>();
        }
        [MenuItem(EXTENSION + "Graphic/Curve")]
        protected static void CreateGraphicCurve()
        {
            GameObject go = CreateGameObject("GraphicCurve", true);

            var component = go.AddComponent<GraphicCurve>();
        }
        [MenuItem(EXTENSION + "Graphic/Gradient")]
        protected static void CreateGraphicGradient()
        {
            GameObject go = CreateGameObject("GraphicGradient", true);

            var component = go.AddComponent<GraphicColorMixed>();
        }
        [MenuItem(EXTENSION + "Graphic/Mirror")]
        protected static void CreateGraphicMirror()
        {
            GameObject go = CreateGameObject("GraphicMirror", true);

            var component = go.AddComponent<GraphicMirror>();
        }
        [MenuItem(EXTENSION + "Image/Fade")]
        protected static void CreateImageFade()
        {
            GameObject go = CreateGameObject("ImageFade", true);

            var component = go.AddComponent<ImageFade>();
        }

        [MenuItem(EXTENSION + "Scroll/InfiniteLoopScrollList")]
        protected static void CreateInfiniteLoopScrollList()
        {
            GameObject go = CreateGameObject("InfiniteLoopScrollList", true);

            var component = go.AddComponent<InfiniteLoopScrollList>();
        }
        [MenuItem(EXTENSION + "Toggle/ToggleDrag")]
        protected static void CreateToggleDrag()
        {
            GameObject go = CreateGameObject("ToggleDrag", true);

            var component = go.AddComponent<ToggleDrag>();
        }
        [MenuItem(EXTENSION + "Range/Number")]
        protected static void CreateRangeNumber()
        {
            GameObject go = CreateGameObject("RangeNumber", true);

            var component = go.AddComponent<RangeNumber>();
        }
        [MenuItem(EXTENSION + "Range/Panel")]
        protected static void CreateRangePanel()
        {
            GameObject go = CreateGameObject("RangePanel", true);

            var component = go.AddComponent<RangePlane>();
        }
        #endregion

        #region Template
        [MenuItem(TEMPLATE + "Background")]
        public static GameObject CreateBackground()
        {
            var background = CreateGameObject("background", true);

            var imgage = background.AddComponent<Image>();

            imgage.color = new Color(0, 0, 0, 0.8f);

            imgage.rectTransform.SetFull();

            return background;
        }
        #endregion

        private static GameObject CreateGameObject(string name, bool ugui = false)
        {
            GameObject go = new GameObject(name);

            go.transform.SetParent(Parent(ugui));

            go.transform.localRotation = Quaternion.identity;

            go.transform.localPosition = Vector3.zero;

            go.transform.localScale = Vector3.one;

            go.layer = ugui ? 5 : 0;

            Selection.activeGameObject = go;

            if (ugui)
            {
                go.AddComponent<RectTransform>();
            }
            return go;
        }

        private static Transform Parent(bool ugui = false)
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

        private static Sprite GetSprite(int index = 0)
        {
            string path;

            switch (index)
            {
                case 0:
                    path = "UI/Skin/UISprite.psd";
                    break;
                case 1:
                    path = "UI/Skin/Background.psd";
                    break;
                case 2:
                    path = "UI/Skin/InputFieldBackground.psd";
                    break;
                case 3:
                    path = "UI/Skin/Knob.psd";
                    break;
                case 4:
                    path = "UI/Skin/Checkmark.psd";
                    break;
                case 5:
                    path = "UI/Skin/DropdownArrow.psd";
                    break;
                case 6:
                    path = "UI/Skin/UIMask.psd";
                    break;
                default:
                    goto case 0;
            }
            return AssetDatabase.GetBuiltinExtraResource<Sprite>(path);
        }
    }
}