using Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIShop : UIBase
    {
        [SerializeField] private TabGroup tab;

        [SerializeField] private List<UIShopBase> shops;

        private void Awake()
        {
            tab.Initialize(OnClickTab);
        }

        private void Start()
        {
            tab.Refresh(new int[3] { 0, 1, 2 });

            Refresh(0);
        }

        public void Refresh(int index)
        {
            UIShopBase shop = shops.Find(x => x.Equal((CounterCategory)index));

            if (shop != null)
            {
                shop.Refresh(ShopLogic.Instance.Get((CounterCategory)index));
            }

            int count = shops.Count;

            for (int i = 0; i < count; i++)
            {
                shops[i].SetActive(shops[i].Equal((CounterCategory)index));
            }
        }

        private void OnClickTab(int index)
        {
            Refresh(index);
        }
    }
}