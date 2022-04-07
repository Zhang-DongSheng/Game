using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public abstract class Tab : ItemBase
    {
        [SerializeField] protected Button button;

        public Action<int, int> callback;

        protected bool select;

        protected int index, value;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        public void Initialize(int index, Action<int, int> action)
        {
            this.index = index; callback = action;
        }

        public void Refresh(int value)
        {
            this.value = value;
        }

        public void Select(int index)
        {
            bool _select = this.index.Equals(index);

            if (select != _select)
            {
                select = _select;

                Switch(select);
            }
        }

        protected virtual void Switch(bool state)
        {

        }

        protected virtual void OnClick()
        {
            callback?.Invoke(index, value);
        }
    }
}