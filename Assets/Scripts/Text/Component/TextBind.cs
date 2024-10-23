using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [DisallowMultipleComponent]
    public class TextBind : MonoBehaviour
    {
        [SerializeField] private string content;

        private Text _component;

        private TextMeshProUGUI _component2;

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
            SetTextImmediately(content);
        }

        private void OnValidate()
        {
            SetText(content);
        }

        private void Relevance()
        {
            if (_relevance) return;

            _relevance = true;

            _component = GetComponent<Text>();

            _component2 = GetComponent<TextMeshProUGUI>();
        }

        private void OnLanguageChange(UnityEngine.EventArgs args)
        {
            if (string.IsNullOrEmpty(content))
            {
                return;
            }
            SetTextImmediately(content);
        }

        public void SetText(string content)
        {
            if (this.content == content)
            {
                return;
            }
            this.content = content;

            SetTextImmediately(content);
        }

        public void SetText(int value)
        {
            content = string.Empty;

            _content = value.ToString();

            SetContent(_content);
        }

        public void SetText(float value, int digits = -1)
        {
            content = string.Empty;

            if (digits > -1)
            {
                _content = string.Format("{0}", Math.Round(value, digits));
            }
            else
            {
                _content = string.Format("{0}", value);
            }
            SetContent(_content);
        }

        protected void SetTextImmediately(string content)
        {
            _content = LanguageManager.Instance.Get(content);

            SetContent(_content);
        }

        protected void SetContent(string content)
        {
            Relevance();
            if (_component != null)
                _component.text = content;
            if (_component2 != null)
                _component2.text = content;
        }
    }
}