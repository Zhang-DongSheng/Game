using Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UIWarehouseContent : MonoBehaviour
    {
        public Action<Prop> callback;

        [SerializeField] private ParentAndPrefab prefab;

        private readonly List<ItemProp> items = new List<ItemProp>();

        public void Refresh(int index)
        {
            var column = WarehouseLogic.Instance.Column(index);

            int count = column.Count;

            for (int i = 0; i < count; i++)
            {
                if (i >= items.Count)
                {
                    ItemProp item = prefab.Create<ItemProp>();
                    item.callback = OnClickProp;
                    items.Add(item);
                }
                items[i].Refresh(column[i]);
            }
            for (int i = count; i < items.Count; i++)
            {
                items[i].SetActive(false);
            }
        }

        private void OnClickProp(Prop prop)
        {
            callback?.Invoke(prop);
        }
    }
}