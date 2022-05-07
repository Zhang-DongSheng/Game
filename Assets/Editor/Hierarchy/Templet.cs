using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.Hierarchy
{
    class Templet
    {
        protected const string UIMENU = "GameObject/UI/Extension/";
        #region UGUI
        [MenuItem(UIMENU + "Button/AlphaButton")]
        protected static void CreateAlphaButton()
        {
            GameObject go = CreateGameObject("AlphaButton", true);

            AlphaButton button = go.AddComponent<AlphaButton>();
        }
        [MenuItem(UIMENU + "Button/ButtonPro")]
        protected static void CreateButtonPro()
        {
            GameObject go = CreateGameObject("ButtonPro", true);

            Image image = go.AddComponent<Image>();

            image.sprite = GetSprite();

            ButtonPro button = go.AddComponent<ButtonPro>();
        }
        [MenuItem(UIMENU + "Button/CodeButton")]
        protected static void CreateCodeButton()
        {
            GameObject go = CreateGameObject("CodeButton", true);

            CodeButton button = go.AddComponent<CodeButton>();
        }
        [MenuItem(UIMENU + "Button/EmptyButton")]
        protected static void CreateEmptyButton()
        {
            GameObject go = CreateGameObject("EmptyButton", true);

            EmptyButton button = go.AddComponent<EmptyButton>();
        }
        [MenuItem(UIMENU + "Button/PolygonImageButton")]
        protected static void CreatePolygonImageButton()
        {
            GameObject go = CreateGameObject("PolygonImageButton", true);

            PolygonImageButton button = go.AddComponent<PolygonImageButton>();
        }
        [MenuItem(UIMENU + "Button/PolygonSpriteButton")]
        protected static void CreatePolygonSpriteButton()
        {
            GameObject go = CreateGameObject("PolygonSpriteButton", true);

            PolygonSpriteButton button = go.AddComponent<PolygonSpriteButton>();
        }
        [MenuItem(UIMENU + "Graphic/Circle/Circle")]
        protected static void CreateCircleCircle()
        {
            GameObject go = CreateGameObject("Circle", true);

            Circle circle = go.AddComponent<Circle>();
        }
        [MenuItem(UIMENU + "Graphic/Circle/CirclePro")]
        protected static void CreateCircleCirclePro()
        {
            GameObject go = CreateGameObject("CirclePro", true);

            CirclePro circle = go.AddComponent<CirclePro>();
        }
        [MenuItem(UIMENU + "Graphic/Circle/Squircle")]
        protected static void CreateCircleSquircle()
        {
            GameObject go = CreateGameObject("Squircle", true);

            Squircle circle = go.AddComponent<Squircle>();
        }
        [MenuItem(UIMENU + "Graphic/Polygon")]
        protected static void CreatePolygon()
        {
            GameObject go = CreateGameObject("Polygon", true);

            Polygon polygon = go.AddComponent<Polygon>();
        }
        [MenuItem(UIMENU + "Graphic/Curve")]
        protected static void CreateGraphicCurve()
        {
            GameObject go = CreateGameObject("GraphicCurve", true);

            GraphicCurve curve = go.AddComponent<GraphicCurve>();
        }
        [MenuItem(UIMENU + "Graphic/Gradient")]
        protected static void CreateGraphicGradient()
        {
            GameObject go = CreateGameObject("GraphicGradient", true);

            GraphicGradient gradient = go.AddComponent<GraphicGradient>();
        }
        [MenuItem(UIMENU + "Graphic/Mirror")]
        protected static void CreateGraphicMirror()
        {
            GameObject go = CreateGameObject("GraphicMirror", true);

            GraphicMirror mirror = go.AddComponent<GraphicMirror>();
        }
        [MenuItem(UIMENU + "Image/Fade")]
        protected static void CreateImageFade()
        {
            GameObject go = CreateGameObject("ImageFade", true);

            ImageFade image = go.AddComponent<ImageFade>();
        }

        [MenuItem(UIMENU + "Scroll/InfiniteLoopScrollList")]
        protected static void CreateInfiniteLoopScrollList()
        {
            GameObject go = CreateGameObject("InfiniteLoopScrollList", true);

            InfiniteLoopScrollList scroll = go.AddComponent<InfiniteLoopScrollList>();
        }
        [MenuItem(UIMENU + "Slider/SliderHelper")]
        protected static void CreateSliderHelper()
        {
            GameObject go = CreateGameObject("SliderHelper", true);

            SliderHelper slider = go.AddComponent<SliderHelper>();
        }
        [MenuItem(UIMENU + "Toggle/ToggleDrag")]
        protected static void CreateToggleDrag()
        {
            GameObject go = CreateGameObject("ToggleDrag", true);

            ToggleDrag drag = go.AddComponent<ToggleDrag>();
        }
        [MenuItem(UIMENU + "Toggle/ToggleGroupHelper")]
        protected static void CreateToggleGroupHelper()
        {
            GameObject go = CreateGameObject("ToggleDrag", true);

            ToggleGroupHelper group = go.AddComponent<ToggleGroupHelper>();
        }
        #endregion
        private static GameObject CreateGameObject(string name, bool ugui = false)
        {
            GameObject go = new GameObject(name);

            Selection.activeGameObject = go;

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