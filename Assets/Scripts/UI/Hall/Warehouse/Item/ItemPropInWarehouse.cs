using Game.Data;
using System;
using UnityEngine;

namespace Game.UI
{
    public class ItemPropInWarehouse : ItemBase
    {
        [SerializeField] private ItemProp m_item;

        [SerializeField] private GameObject m_reddot;

        [SerializeField] private GameObject m_select;

        private Action<uint> _callback;

        protected override void OnAwake()
        {
            m_item.callback = OnClick;
        }

        public void Refresh(Prop prop, uint select, Action<uint> callback)
        {
            _callback = callback;

            m_item.Refresh(prop);

            SetActive(m_select, prop.parallelism == select);
        }

        private void OnClick(uint propID)
        {
            _callback?.Invoke(propID);
        }
    }
}