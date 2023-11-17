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

        public override void Refresh(UIParameter parameter)
        {
            List<int> _shops = new List<int>();

            int count = m_shops.Count;

            for (int i = 0; i < count; i++)
            {
                if (ShopLogic.Instance.Exists(m_shops[i].shopID))
                {
                    _shops.Add(m_shops[i].shopID);
                }
            }
            m_menu.Refresh(_shops.ToArray());

            m_menu.Select(_shops[0], true);

            Refresh();
        }

        public void Refresh()
        {
            int count = m_shops.Count;

            for (int i = 0; i < count; i++)
            {
                if (ShopLogic.Instance.Exists(m_shops[i].shopID))
                {
                    m_shops[i].Refresh();
                }
            }
        }

        private void OnClickTab(int index)
        {
            int count = m_shops.Count;

            for (int i = 0; i < count; i++)
            {
                m_shops[i].SetActive(m_shops[i].Equal(index));
            }
        }
    }
}