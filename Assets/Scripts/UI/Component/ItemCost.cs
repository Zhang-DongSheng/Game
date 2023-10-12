using Data;
using UnityEngine;

namespace Game.UI
{
    public class ItemCost : ItemBase
    {
        [SerializeField] private ImageBind imgIcon;

        [SerializeField] private TextBind txtNumber;

        public void Refresh(Cost cost)
        {
            if (cost == null) return;

            if (cost.consume == Consume.Recharge) return;

            PropInformation prop = DataManager.Instance.Load<DataProp>().Get(cost.coin);

            imgIcon.SetSprite(prop.icon);

            txtNumber.SetText(cost.number);
        }
    }
}