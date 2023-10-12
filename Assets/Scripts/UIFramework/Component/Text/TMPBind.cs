using System;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    [DisallowMultipleComponent, RequireComponent(typeof(TextMeshProUGUI))]
    public class TMPBind : MonoBehaviour
    {
        [SerializeField] private string content;

        private string m_content;

        private TextMeshProUGUI m_text;

        private void Awake()
        {
            EventManager.Register(EventKey.Language, OnLanguageChange);
        }

        private void OnDestroy()
        {
            EventManager.Unregister(EventKey.Language, OnLanguageChange);
        }

        private void OnValidate()
        {
            SetText(content);
        }

        private void OnLanguageChange(EventMessageArgs args)
        {
            SetText(content, true, true);
        }

        public void SetText(string content, bool language = true, bool force = false)
        {
            if (m_content == content && !force) return;

            m_content = this.content = content;

            if (m_text == null)
                m_text = GetComponent<TextMeshProUGUI>();
            if (language)
            {
                m_text.SetText(LanguageManager.Instance.Get(content));
            }
            else
            {
                m_text.SetText(content);
            }
        }

        public void SetText(int value)
        {
            SetText(value.ToString(), false);
        }

        public void SetText(float value, int digits = -1)
        {
            if (digits > -1)
            {
                SetText(string.Format("{0}", Math.Round(value, digits)), false);
            }
            else
            {
                SetText(string.Format("{0}", value), false);
            }
        }
    }
}