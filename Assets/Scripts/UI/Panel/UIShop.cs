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
            tab.Refresh(new int[5] { 1, 2, 3, 4, 5 });

            Refresh(0);
        }

        public void Refresh(int index)
        {
            UIShopBase shop = shops.Find(x => x.Equal((CounterEnum)index));

            if (shop != null)
            {
                shop.Refresh(ShopLogic.Instance.Get(index));
            }
        }

        private void OnClickTab(int index)
        {
            Refresh(index);
        }
    }
}