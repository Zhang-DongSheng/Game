using Game.Logic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class SubWarehouseContent : ItemBase
    {
        public Action<uint> callback;

        [SerializeField] private ListLayoutGroup layout;

        [SerializeField] private ItemPropInWarehouse prefab;

        private uint select;

        public void Refresh(int index)
        {
            var column = WarehouseLogic.Instance.GetPropsByCategory(index);

            int count = column.Count;

            layout.SetData(prefab, column, (index, item, data) =>
            {
                item.Refresh(data, select, OnClickProp);
            });
        }

        private void OnClickProp(uint propID)
        {
            select = propID;

            callback?.Invoke(propID);

            layout.ForceUpdateContent();
        }
    }
}