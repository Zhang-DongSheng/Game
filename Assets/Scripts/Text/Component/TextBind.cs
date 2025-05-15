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

        public bool language = true;

        private Text _text;

        private TextMeshProUGUI _textmp;

        private string _content;

        private bool _relevance = true;

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
            SetText(content, language);
        }

        private void OnValidate()
        {
            SetText(content, language);
        }

        private void OnLanguageChange(UnityEngine.EventArgs args)
        {
            if (string.IsNullOrEmpty(content))
            {
                return;
            }
            SetText(content, language);
        }

        public void SetText(string content, bool language = true)
        {
            if (this.content.Equals(content)) return;

            this.content = content;

            this.language = language;

            _content = LanguageManager.Instance.Get(content);

            SetTextImmediately(_content);
        }

        public void SetTextWithParameters(string content, params object[] parameters)
        {
            if (this.content.Equals(content)) return;

            this.content = content;

            _content = LanguageManager.Instance.Get(content);

            _content = string.Format(_content, parameters);

            SetTextImmediately(_content);
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

            SetTextImmediately(_content);
        }

        private void SetTextImmediately(string content)
        {
            if (_relevance)
            {
                _relevance = false;
                _text = GetComponent<Text>();
                _textmp = GetComponent<TextMeshProUGUI>();
            }
            if (_text != null)
                _text.text = content;
            if (_textmp != null)
                _textmp.text = content;
        }
    }
}