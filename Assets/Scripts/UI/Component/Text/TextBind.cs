using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Text))]
    public class TextBind : MonoBehaviour
    {
        public string content;

        private Text m_text;

        private void Awake()
        {
            SetText(content);
        }

        public void SetText(string content, bool language = true)
        {
            if (this.content.Equals(content)) return;

            this.content = content;

            if (m_text == null)
                m_text = GetComponent<Text>();

            if (language)
            {
                m_text.SetText(TextManager.Instance.Get(content));
            }
            else
            {
                m_text.SetText(content);
            }
        }

        public void SetText(int value)
        {
            if (m_text == null)
                m_text = GetComponent<Text>();
            m_text.SetText(value);
        }

        public void SetText(float value, int digits = -1)
        {
            if (m_text == null)
                m_text = GetComponent<Text>();
            m_text.SetText(value, digits);
        }
    }
}