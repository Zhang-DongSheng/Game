using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIColorPicker : ItemBase
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

        [SerializeField] private Slider slider;

        [SerializeField] private Image imageRibbon;

        [SerializeField] private Image imagePicker;

        [SerializeField] private RectTransform handle;

        private Texture2D texture;

        private Vector2 position, space;

        private Color c0, c1, c2, c3, output;

        private float x, y, z, w;

        private Status status;

        public UnityEvent<Color> onValueChanged;

        protected override void OnAwake()
        {
            slider.minValue = 0;

            slider.maxValue = colors.Length - 1;

            slider.onValueChanged.AddListener(OnValueChanged);

            space = imagePicker.rectTransform.rect.size;

            position = Vector2.zero;
        }

        private void Start()
        {
            Init();
        }

        protected override void OnUpdate(float delta)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (InSide(imagePicker.rectTransform, out _))
                {
                    status = Status.Picker;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (status == Status.Picker)
                {
                    status = Status.Idle;
                }
            }
            switch (status)
            {
                case Status.Picker:
                    {
                        Picker();
                    }
                    break;
            }
        }

        private void Init()
        {
            #region Ribbon
            Texture2D _texture = new Texture2D(1, colors.Length);

            for (int i = 0; i < colors.Length; i++)
            {
                _texture.SetPixel(0, i, colors[i]);
            }
            _texture.Apply();

            imageRibbon.sprite = Sprite.Create(_texture, new Rect(0, 0.5f, 1, 6), new Vector2(0.5f, 0.5f));
            #endregion

            #region Picker
            texture = new Texture2D(2, 2);

            imagePicker.sprite = Sprite.Create(texture, new Rect(0.5f, 0.5f, 1, 1), new Vector2(0.5f, 0.5f));
            #endregion

            OnValueChanged(0);
        }

        private void OnValueChanged(float value)
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

            Picker(false);
        }

        private void Picker(bool drag = true)
        {
            if (drag)
            {
                InSide(imagePicker.rectTransform, out position);

                handle.anchoredPosition = position;
            }

            x = position.x / space.x + 0.5f;

            y = position.y / space.y + 0.5f;

            z = 1 - x; w = 1 - y;

            c0 = z * w * picker[0];

            c1 = x * w * picker[1];

            c2 = z * y * picker[2];

            c3 = x * y * picker[3];

            output = c0 + c1 + c2 + c3;

            onValueChanged?.Invoke(output);
        }

        private static bool InSide(RectTransform target, out Vector2 result)
        {
            Vector2 point = target.InverseTransformPoint(Input.mousePosition);

            result.x = Mathf.Clamp(point.x, target.rect.min.x, target.rect.max.x);

            result.y = Mathf.Clamp(point.y, target.rect.min.y, target.rect.max.y);

            return target.rect.Contains(point);
        }

        enum Status
        {
            Idle,
            Picker,
        }
    }
}