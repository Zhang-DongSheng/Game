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

        private void OnEnable()
        {
            SetText(content, false);
        }

        private void OnValidate()
        {
            SetText(content, false);
        }

        private void OnLanguageChange(EventMessageArgs args)
        {
            SetText(content, true);
        }

        public void SetText(string content, bool force = false)
        {
            if (string.IsNullOrEmpty(content)) return;

            if (_content == content && !force) return;

            _content = content;

            if (_component == null)
                _component = GetComponent<Text>();
            _component.SetText(LanguageManager.Instance.Get(content));
        }

        public void SetText(int value)
        {
            SetTextImmediately(value.ToString());
        }

        public void SetText(float value, int digits = -1)
        {
            if (digits > -1)
            {
                SetTextImmediately(string.Format("{0}", Math.Round(value, digits)));
            }
            else
            {
                SetTextImmediately(string.Format("{0}", value));
            }
        }

        public void SetTextImmediately(string content)
        {
            this.content = string.Empty;

            if (_content == content) return;

            _content = content;

            if (_component == null)
                _component = GetComponent<Text>();
            _component.SetText(content);
        }
    }
}