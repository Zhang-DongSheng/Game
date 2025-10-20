using Game.Data;
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

            if (table == null || table.props.Count == 0) return;

            m_prop.Refresh(table.props[0]);

            m_cost.Refresh(table.cost);

            var status = Status.Available;

            if (commodity.count > 0)
            {
                status = commodity.purchased < commodity.count ? Status.Available : Status.Claimed;
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

                int count = table.props.Count;

                for (int i = 0; i < count; i++)
                {
                    reward.props.Add(new Prop(0, table.props[i].x, table.props[i].y));
                }
                UIQuickEntry.OpenRewardView(reward);
            });
        }
    }
}