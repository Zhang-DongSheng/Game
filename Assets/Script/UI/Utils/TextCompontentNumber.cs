using TMPro;

namespace UnityEngine.UI
{
    public class TextCompontentNumber : MonoBehaviour
    {
        private readonly int Million = 1000 * 1000;

        private readonly int Thousand = 1000;

        [SerializeField] private Text m_text;

        [SerializeField] private TextMeshProUGUI m_textMesh;

        [SerializeField] private Digits m_digits;

        [SerializeField, Range(0.1f, 10)] private float m_speed;

        private float current, origin, destination, step;

        private bool scroll;

        private void Awake()
        {
            if (m_text == null)
                m_text = GetComponentInChildren<Text>();
            if (m_textMesh != null)
                m_textMesh = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (scroll)
            {
                step += m_speed * Time.deltaTime;

                current = Mathf.Lerp(origin, destination, step);

                if (step > 1)
                {
                    step = 0; scroll = false;
                }

                SetNumber(current);
            }
        }

        public void SetText(string value)
        {
            if (m_text != null)
                m_text.text = value;
            if (m_textMesh != null)
                m_textMesh.text = value;
        }

        public void SetNumber(float value)
        {
            SetText(Format(value));
        }

        public void SetScrollNumber(float value)
        {
            origin = 0;
            destination = value;
            step = 0;
            scroll = true;
        }

        private string Format(float value)
        {
            string number;

            if (value >= Million)
            {
                number = string.Format("{0}M", Decimal(value / Million));
            }
            else if (value >= Thousand)
            {
                number = string.Format("{0}K", Decimal(value / Thousand));
            }
            else
            {
                number = Decimal(value).ToString();
            }

            return number;
        }

        private float Decimal(float value)
        {
            switch (m_digits)
            {
                case Digits.None:
                    return (int)value;
                case Digits.One:
                case Digits.Two:
                case Digits.Three:
                    return (float)System.Math.Round(value, (int)m_digits);
                default:
                    return value;
            }
        }

        enum Digits
        { 
            None,
            One,
            Two,
            Three,
            All,
        }
    }
}