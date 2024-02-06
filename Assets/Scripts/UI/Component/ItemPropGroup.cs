using Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ItemPropGroup : ItemBase
    {
        [SerializeField] private Transform parent;

        [SerializeField] private ItemProp prefab;

        private readonly List<ItemProp> items = new List<ItemProp>();

        public void Refresh(List<Prop> props)
        {
            int count = props.Count;

            for (int i = 0; i < count; i++)
            {
                if (i >= items.Count)
                {
                    items.Add(GameObject.Instantiate<ItemProp>(prefab, parent));
                }
                items[i].Refresh(props[i]);
            }
            for (int i = count; i < items.Count; i++)
            {
                items[i].SetActive(false);
            }
        }

        public void Refresh(int count, Action<int, ItemProp> callback)
        {
            for (int i = 0; i < count; i++)
            {
                if (i >= items.Count)
                {
                    items.Add(GameObject.Instantiate<ItemProp>(prefab, parent));
                }
                callback?.Invoke(i, items[i]);
            }
            for (int i = count; i < items.Count; i++)
            {
                items[i].SetActive(false);
            }
        }
    }
}