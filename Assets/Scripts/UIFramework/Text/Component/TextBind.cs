using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [DisallowMultipleComponent]
    public class TextBind : MonoBehaviour
    {
        public string content = string.Empty;

        private Text _text;

        private TextMeshProUGUI _textmp;

        private string _content;

        private bool _relevance = false;

        private void Awake()
        {
            EventDispatcher.Register(UIEvent.Language, OnLanguageChange);
        }

        private void OnDestroy()
        {
            EventDispatcher.Unregister(UIEvent.Language, OnLanguageChange);
        }

        private void Start()
        {
            SwitchLanguage();

            SetText(content, true);
        }

        private void OnValidate()
        {
            SetText(content);
        }

        private void Relevance()
        {
            if (_relevance) return;

            _relevance = true;

            _text = GetComponent<Text>();

            _textmp = GetComponent<TextMeshProUGUI>();
        }

        private void OnLanguageChange(UnityEngine.EventArgs args)
        {
            SwitchLanguage();

            if (string.IsNullOrEmpty(content))
            {
                return;
            }
            SetText(content, true);
        }

        public void SetText(string content, bool force = false)
        {
            if (string.IsNullOrEmpty(content)) return;

            if (this.content.Equals(content) && !force) return;

            this.content = content;

            _content = LanguageManager.Instance.Get(content);

            SetContent(_content);
        }

        public void SetTextWithParameters(string content, params object[] parameters)
        {
            if (this.content.Equals(content)) return;

            this.content = content;

            _content = LanguageManager.Instance.Get(content);

            _content = string.Format(_content, parameters);

            SetContent(_content);
        }

        public void SetTextImmediately(string content)
        {
            this.content = string.Empty;

            _content = content;

            SetContent(_content);
        }

        public void SetNumber(float value, int digits = -1)
        {
            if (digits > -1)
            {
                content = string.Format("{0}", Math.Round(value, digits));
            }
            else
            {
                content = string.Format("{0}", value);
            }
            _content = content;

            SetContent(_content);
        }

        private void SetContent(string content)
        {
            Relevance();
            if (_text != null)
                _text.text = content;
            if (_textmp != null)
                _textmp.text = content;
        }

        private void SwitchLanguage()
        {
            Relevance();
            var font = LanguageManager.Instance.Font;
            if (font == null) return;
            if (_textmp != null && _textmp.font != font)
                _textmp.font = font;
        }
    }
}