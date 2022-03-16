using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemTab : ItemBase
    {
        [SerializeField] private Button button;

        protected Action<int> callback;

        protected bool select;

        protected int index;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        public void Initialize(int index, Action<int> action)
        {
            this.index = index;

            callback = action;
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
            callback?.Invoke(index);
        }
    }
}