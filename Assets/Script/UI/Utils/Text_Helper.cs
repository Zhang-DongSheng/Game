using System;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Text))]
    public class Text_Helper : MonoBehaviour
    {
        public Text m_text;

        public string key, value;

        public bool m_language;

        public bool m_scroll;

        private float number_current;
        private float number_target;
        private float number_speed;
        private float number_ratio;

        private void Awake()
        {
            m_text = GetComponent<Text>();
        }

        private void Start()
        {
            Refresh();
        }

        private void Update()
        {
            if (m_scroll)
            {
                Scroll();
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isEditor)
            {
                Refresh();
            }
        }
#endif

        public void SetText(string value, bool language = true)
        {
            m_language = language;

            if (m_language)
            {
                key = value;
            }
            else
            {
                this.value = value;
            }

            Refresh();
        }

        public void SetNumber(int target, bool scroll = false, float speed = 0.5f)
        {
            m_scroll = scroll;

            if (m_scroll)
            {
                int.TryParse(value, out int current);
                value = target.ToString();
                number_current = current;
                number_target = target;
                number_speed = speed;
                number_ratio = 0;
            }
            else
            {
                m_text.text = target.ToString();
            }
        }

        public void SetColor(Color color)
        {
            m_text.color = color;
        }

        private void Refresh()
        {
            if (m_language)
            {
                m_text.text = DataHelper.Instance.Word(key);
            }
            else
            {
                m_text.text = value;
            }
        }

        private void Scroll()
        {
            number_ratio += Time.deltaTime * number_speed;

            if (number_ratio >= 1)
            {
                number_current = number_target; m_scroll = false;
            }
            else
            {
                number_current = Mathf.Lerp(number_current, number_target, number_ratio);
            }

            m_text.text = Convert.ToInt32(number_current).ToString();
        }
    }
}