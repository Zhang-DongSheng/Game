using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class SubWarehouseIntroduce : ItemBase
    {
        [SerializeField] private ItemProp m_item;

        [SerializeField] private Text m_label;

        public void Refresh(Prop prop)
        {
            m_item.Refresh(prop);

            var table = DataProp.Get(prop.parallelism);

            m_label.text = table.description;
        }
    }
}