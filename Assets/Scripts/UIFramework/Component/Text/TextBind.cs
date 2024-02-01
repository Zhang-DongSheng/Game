using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Text))]
    public class TextBind : MonoBehaviour
    {
        [SerializeField] private string content;

        private Text _component;

        private string _content;

        private void Awake()
        {
            EventManager.Register(EventKey.Language, OnLanguageChange);
        }

        private void OnDestroy()
        {
            EventManager.Unregister(EventKey.Language, OnLanguageChange);
        }

        private void Start()
        {
            SetTextImmediately(content);
        }

        private void OnValidate()
        {
            SetText(content);
        }

        private void OnLanguageChange(EventMessageArgs args)
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
            string key = value.ToString();

            _content = LanguageManager.Instance.Get(key);

            this.content = _content != key ? key : string.Empty;

            if (_component == null)
                _component = GetComponent<Text>();
            _component.text = _content;
        }

        public void SetText(float value, int digits = -1)
        {
            this.content = string.Empty;

            if (digits > -1)
            {
                _content = string.Format("{0}", Math.Round(value, digits));
            }
            else
            {
                _content = string.Format("{0}", value);
            }
            if (_component == null)
                _component = GetComponent<Text>();
            _component.text = _content;
        }

        protected void SetTextImmediately(string content)
        {
            if (_component == null)
                _component = GetComponent<Text>();
            _component.text = LanguageManager.Instance.Get(content);
        }
    }
}