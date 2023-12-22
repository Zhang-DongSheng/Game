using Data;
using UnityEngine;

namespace Game.UI
{
    public class ItemCost : ItemBase
    {
        [SerializeField] private ImageBind imgIcon;

        [SerializeField] private TextBind txtNumber;

        protected uint coin;

        protected float amount;

        public void Refresh(uint coin, float amount)
        {
            this.coin = coin;

            this.amount = amount;

            Refresh();
        }

        protected void Refresh()
        {
            PropInformation prop = DataProp.Get(coin);

            imgIcon.SetSprite(prop.icon);

            txtNumber.SetText(amount);
        }
    }
}