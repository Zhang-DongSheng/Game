using Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemSignIn : ItemBase
    {
        public Action<int> callback;

        [SerializeField] private ItemProp m_item;

        [SerializeField] private ItemStatus m_status;

        [SerializeField] private Button button;

        private int _index;

        protected override void OnAwake()
        {
            button.onClick.AddListener(OnClick);
        }

        public void Refresh(int index)
        {
            _index = index;

            m_item.Refresh(1001 + (uint)index);

            m_status.Refresh(Status.Available);
        }

        private void OnClick() 
        {
            callback?.Invoke(_index);
        }
    }
}