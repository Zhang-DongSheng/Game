using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIShop : UIBase
    {
        [SerializeField] private ItemToggleGroup m_menu;

        [SerializeField] private ScrollRect m_scroll;

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
                else
                {
                    m_shops[i].SetActive(false);
                }
            }
        }

        private void OnClickTab(int index)
        {
            var shop = m_shops.Find(x => x.shopID == index);

            var child = shop.RectTransform;

            var position = child.anchoredPosition;

            position.x += child.rect.width * 0.5f;

            position.x -= m_scroll.viewport.rect.width * 0.5f;

            position.x = Mathf.Clamp(position.x, 0, m_scroll.content.rect.width - m_scroll.viewport.rect.width);

            position.x *= -1;

            position.y = 0;

            m_scroll.content.anchoredPosition = position;
        }
    }
}