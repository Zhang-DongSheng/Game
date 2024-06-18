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

            m_prop.Refresh(table.rewards[0].x, table.rewards[0].y);

            m_cost.Refresh(table.cost);

            var status = Status.Available;

            if (table.number > 0)
            {
                status = commodity.purchased < table.number ? Status.Available : Status.Claimed;
            }
            m_status.Refresh(status);
        }

        private void OnClick()
        {
            UIQuickEntry.OpenConfirmView("Tips", "Confirm", () =>
            {
                var reward = new Reward()
                {
                    props = new List<Prop>()
                };
                var table = DataCommodity.Get(commodity.primary);

                int count = table.rewards.Count;

                for (int i = 0; i < count; i++)
                {
                    reward.props.Add(new Prop(0, table.rewards[i].x, table.rewards[i].y));
                }
                UIQuickEntry.OpenRewardView(reward);
            });
        }
    }
}