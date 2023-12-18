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

            var table = DataManager.Instance.Load<DataCommodity>().Get(commodity.primary);

            if (table == null) return;

            m_prop.Refresh(table.rewards[0]);

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
                var table = DataManager.Instance.Load<DataCommodity>().Get(commodity.primary);

                for (int i = 0; i < table.rewards.Count; i++)
                {
                    reward.props.Add(new Prop(table.rewards[i]));
                }
                UIQuickEntry.OpenUIReward(reward);
            });
        }
    }
}