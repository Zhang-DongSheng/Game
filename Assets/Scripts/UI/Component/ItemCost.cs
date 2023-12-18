using Data;
using UnityEngine;

namespace Game.UI
{
    public class ItemCost : ItemBase
    {
        [SerializeField] private ImageBind imgIcon;

        [SerializeField] private TextBind txtNumber;

        protected int coin;

        protected float amount;

        public void Refresh(int coin, int amount)
        {
            this.coin = coin;

            this.amount = amount;

            Refresh();
        }

        protected void Refresh()
        {
            PropInformation prop = DataManager.Instance.Load<DataProp>().Get(coin);

            imgIcon.SetSprite(prop.icon);

            txtNumber.SetText(amount);
        }
    }
}