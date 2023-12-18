using Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIWarehouseContent : ItemBase
    {
        public Action<Prop> callback;

        [SerializeField] private ListLayoutGroup layout;

        [SerializeField] private ItemPropInWarehouse prefab;

        private uint select;

        public void Refresh(int index)
        {
            var column = WarehouseLogic.Instance.GetPropsByCategory(index);

            int count = column.Count;

            layout.SetData(prefab, column, (index, item, prop) =>
            {
                item.Refresh(column[index], OnClickProp, select);
            });
        }

        private void OnClickProp(Prop prop)
        {
            select = prop.identification;

            callback?.Invoke(prop);

            layout.ForceUpdateContent();
        }
    }
}