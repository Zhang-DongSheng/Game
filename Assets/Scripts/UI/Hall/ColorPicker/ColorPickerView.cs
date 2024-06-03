using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ColorPickerView : ViewBase
    {
        private readonly Color[] colors = new Color[]
        {
            Color.red,
            Color.yellow,
            Color.green,
            Color.cyan,
            Color.blue,
            Color.magenta,
            Color.red,
        };

        private readonly Color[] picker = new Color[]
        {
            new Color( 0, 0, 0 ),
            new Color( 0, 0, 0 ),
            new Color( 1, 1, 1 ),
            new Color( 1, 1, 1 ),
        };

        [SerializeField] private Image imgRibbon;

        [SerializeField] private Image imgPicker;

        [SerializeField] private Image imgColor;

        [SerializeField] private Text txtColor;

        [SerializeField] private Slider m_ribbon;

        [SerializeField] private Slider m_alpha;

        [SerializeField] private SliderV2 m_picker;

        private Texture2D texture;

        private Vector2 position, space;

        private Color c0, c1, c2, c3, output;

        private float alpha;

        private float x, y, z, w;

        public Action<Color> onValueChanged;

        protected override void OnAwake()
        {
            space = imgPicker.rectTransform.rect.size;

            position = Vector2.zero;

            m_ribbon.minValue = 0;

            m_ribbon.maxValue = colors.Length - 1;

            m_ribbon.direction = Slider.Direction.BottomToTop;

            m_ribbon.onValueChanged.AddListener(OnValueChangedRibbon);

            m_alpha.onValueChanged.AddListener(OnValueChangedAlpha);

            m_picker.onValueChanged.AddListener(OnDrag);

            #region Ribbon
            Texture2D _texture = new Texture2D(1, colors.Length);

            for (int i = 0; i < colors.Length; i++)
            {
                _texture.SetPixel(0, i, colors[i]);
            }
            _texture.Apply();

            imgRibbon.sprite = Sprite.Create(_texture, new Rect(0, 0.5f, 1, 6), new Vector2(0.5f, 0.5f));
            #endregion

            #region Picker
            texture = new Texture2D(2, 2);

            imgPicker.sprite = Sprite.Create(texture, new Rect(0.5f, 0.5f, 1, 1), new Vector2(0.5f, 0.5f));
            #endregion

            OnValueChangedRibbon(0);
        }

        public override void Refresh(UIParameter parameter)
        {
            output = parameter.Get<Color>("color");

            alpha = parameter.Get<float>("alpha");

            onValueChanged = parameter.Get<Action<Color>>("callback");

            Refresh();
        }

        private void Refresh()
        {
            output.a = alpha;

            imgColor.color = output;

            txtColor.text = "#" + ColorUtility.ToHtmlStringRGBA(output);
        }

        private void OnValueChangedRibbon(float value)
        {
            int pre = Mathf.FloorToInt(value);

            int next = Mathf.CeilToInt(value);

            picker[3] = Color.Lerp(colors[pre], colors[next], value - (float)pre);

            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 2; i++)
                {
                    texture.SetPixel(i, j, picker[i + j * 2]);
                }
            }
            texture.Apply();

            Picker();
        }

        private void OnValueChangedAlpha(float value)
        {
            alpha = value;

            Picker();
        }

        private void OnDrag(Vector2 interval)
        {
            position = interval * space * 0.5f;

            Picker();
        }

        private void Picker()
        {
            x = position.x / space.x + 0.5f;

            y = position.y / space.y + 0.5f;

            z = 1 - x; w = 1 - y;

            c0 = z * w * picker[0];

            c1 = x * w * picker[1];

            c2 = z * y * picker[2];

            c3 = x * y * picker[3];

            output = c0 + c1 + c2 + c3;

            output.a = alpha;

            onValueChanged?.Invoke(output);

            Refresh();
        }
    }
}