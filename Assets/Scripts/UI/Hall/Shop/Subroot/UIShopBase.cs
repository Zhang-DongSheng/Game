using Data;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public abstract class UIShopBase : ItemBase
    {
        public int shopID;

        [SerializeField] protected PrefabTemplateBehaviour prefab;

        protected readonly List<ItemCommodity> commodities = new List<ItemCommodity>();

        public virtual void Refresh()
        {
            var shop = ShopLogic.Instance.Get(shopID);

            int count = shop.commodities.Count;

            for (int i = 0; i < count; i++)
            {
                if (i >= commodities.Count)
                {
                    commodities.Add(prefab.Create<ItemCommodity>());
                }
                commodities[i].Refresh(shop.commodities[i]);
            }
            for (int i = count; i < commodities.Count; i++)
            {
                commodities[i].SetActive(false);
            }
        }

        public bool Equal(int shop)
        {
            return this.shopID == shop;
        }
    }
}
