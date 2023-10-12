using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [DisallowMultipleComponent, RequireComponent(typeof(Text))]
    public class TextBind : MonoBehaviour
    {
        [SerializeField] private string content;

        [SerializeField] private bool language = true;

        private Text _text;

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
            SetText(content, language);
        }

        private void OnLanguageChange(EventMessageArgs args)
        {
            SetText(content, language);
        }

        public void SetText(string content, bool language = true)
        {
            if (this.content == content && this.language == language) return;

            this.content = content;

            this.language = language;

            if (_text == null)
                _text = GetComponent<Text>();
            // ÊÇ·ñÊÇ¶àÓïÑÔ
            if (language)
            {
                _text.SetText(LanguageManager.Instance.Get(content));
            }
            else
            {
                _text.SetText(content);
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