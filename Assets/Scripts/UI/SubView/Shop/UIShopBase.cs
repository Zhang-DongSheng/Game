using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Game.UI
{
    public abstract class UIShopBase : ItemBase
    {
        [SerializeField] protected CounterCategory category;

        [SerializeField] protected PrefabWithParent prefab;

        protected readonly List<ItemCommodity> commodities = new List<ItemCommodity>();

        public virtual void Refresh(Counter cabinet)
        {
            int count = cabinet.commodities.Count;

            for (int i = 0; i < count; i++)
            {
                if (i >= commodities.Count)
                {
                    commodities.Add(prefab.Create<ItemCommodity>());
                }
                commodities[i].Refresh(cabinet.commodities[i]);
            }
            for (int i = count; i < commodities.Count; i++)
            {
                commodities[i].SetActive(false);
            }
        }

        public bool Equal(CounterCategory category)
        {
            return this.category == category;
        }
    }
}
