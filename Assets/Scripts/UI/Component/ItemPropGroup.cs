using Game.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ItemPropGroup : ItemBase
    {
        [SerializeField] private PrefabTemplate prefab;

        private readonly List<ItemProp> items = new List<ItemProp>();

        public void Refresh(List<Prop> props)
        {
            int count = props.Count;

            for (int i = 0; i < count; i++)
            {
                if (i >= items.Count)
                {
                    items.Add(prefab.Create<ItemProp>());
                }
                items[i].Refresh(props[i]);
            }
            for (int i = count; i < items.Count; i++)
            {
                items[i].SetActive(false);
            }
        }

        public void Refresh(List<UIntPair> list)
        {
            int count = list.Count;

            for (int i = 0; i < count; i++)
            {
                if (i >= items.Count)
                {
                    items.Add(prefab.Create<ItemProp>());
                }
                items[i].Refresh(list[i].x, list[i].y);
            }
            for (int i = count; i < items.Count; i++)
            {
                items[i].SetActive(false);
            }
        }
    }
}