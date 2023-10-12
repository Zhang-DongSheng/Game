using Data;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UIShop : UIBase
    {
        [SerializeField] private ItemToggleGroup m_menu;

        [SerializeField] private List<UIShopBase> m_shops;

        protected override void OnAwake()
        {
            m_menu.callback = OnClickTab;
        }

        private void Start()
        {
            m_menu.Initialize(3);

            m_menu.Select(0, true);
        }

        public void Refresh(int index)
        {
            UIShopBase shop = m_shops.Find(x => x.Equal((CounterCategory)index));

            if (shop != null)
            {
                shop.Refresh(ShopLogic.Instance.Get((CounterCategory)index));
            }
            int count = m_shops.Count;

            for (int i = 0; i < count; i++)
            {
                m_shops[i].SetActive(m_shops[i].Equal((CounterCategory)index));
            }
        }

        private void OnClickTab(int index)
        {
            Refresh(index);
        }
    }
}