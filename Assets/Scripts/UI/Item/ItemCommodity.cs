using Data;
using UnityEngine;

namespace Game.UI
{
    public class ItemCommodity : ItemBase
    {
        [SerializeField] private ItemProp m_prop;

        [SerializeField] private ItemCost m_cost;

        [SerializeField] private ItemStatus m_status;

        public void Refresh(Commodity commodity)
        {
            m_prop.Refresh(commodity.props[0]);

            m_cost.Refresh(commodity.cost);

            m_status.Refresh(commodity.status);
        }
    }
}