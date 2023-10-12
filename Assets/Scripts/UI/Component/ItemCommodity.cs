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

            m_prop.Refresh(commodity.props[0]);

            m_cost.Refresh(commodity.cost);

            m_status.Refresh(commodity.status);
        }

        private void OnClick()
        {
            UIQuickEntry.OpenUIConfirm("Tips", "ȷ�Ϲ�����Ʒ", () =>
            {
                UIQuickEntry.OpenUIReward(new Reward()
                {
                    title = "����ɹ�",
                    props = new List<Prop>(commodity.props)
                });
            });
        }
    }
}