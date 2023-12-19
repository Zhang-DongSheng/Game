using Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemPropInWarehouse : ItemBase
    {
        [SerializeField] private ItemProp m_item;

        [SerializeField] private GameObject m_reddot;

        [SerializeField] private GameObject m_select;

        [SerializeField] private Button button;

        private Action<Prop> _callback;

        private Prop _prop;

        protected override void OnAwake()
        {
            button.onClick.AddListener(OnClick);
        }

        public void Refresh(Prop prop, uint select, Action<Prop> callback)
        {
            _prop = prop;

            _callback = callback;

            m_item.Refresh(prop);

            SetActive(m_select, prop.identification == select);
        }

        private void OnClick()
        {
            _callback?.Invoke(_prop);
        }
    }
}