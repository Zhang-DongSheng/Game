using Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemCommodity : ItemBase
    {
        [SerializeField] private ItemProp m_prop;

        [SerializeField] private ItemCost m_cost;

        [SerializeField] private ItemStatus m_status;

        [SerializeField] private Button btnBuy;

        private Commodity commodity;

        protected override void OnAwake()
        {
            btnBuy.onClick.AddListener(OnClick);
        }

        public void Refresh(Commodity commodity)
        {
            this.commodity = commodity;

            var table = DataCommodity.Get(commodity.primary);

            if (table == null || table.rewards.Count == 0) return;

            m_prop.Refresh(table.rewards[0].x, (int)table.rewards[0].y);

            m_cost.Refresh(table.cost, table.price);

            m_status.Refresh(commodity.status);
        }

        private void OnClick()
        {
            UIQuickEntry.OpenUIConfirm("Tips", "Confirm", () =>
            {
                var reward = new Reward()
                {
                    props = new List<Prop>()
                };
                var table = DataCommodity.Get(commodity.primary);

                int count = table.rewards.Count;

                for (int i = 0; i < count; i++)
                {
                    reward.props.Add(new Prop(0, table.rewards[i].x, (int)table.rewards[i].y));
                }
                UIQuickEntry.OpenUIReward(reward);
            });
        }
    }
}