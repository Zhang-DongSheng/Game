using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ItemTabGroup : MonoBehaviour
    {
        public Action<int> callback;

        [SerializeField] private ParentAndPrefab prefab;

        private readonly List<ItemTab> items = new List<ItemTab>();

        public void Initialize(int count, int index = 0)
        {
            for (int i = 0; i < count; i++)
            {
                if (i >= items.Count)
                {
                    ItemTab item = prefab.Create<ItemTab>();
                    item.Initialize(i, OnClick);
                    items.Add(item);
                }
            }
            if (index > -1) OnClick(index);
        }

        private void OnClick(int index)
        {
            int count = items.Count;

            for (int i = 0; i < count; i++)
            {
                if (items[i] != null)
                {
                    items[i].Select(index);
                }
            }
            callback?.Invoke(index);
        }
    }
}