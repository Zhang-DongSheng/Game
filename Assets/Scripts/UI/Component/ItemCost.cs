using Game.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// ÏûºÄ
    /// </summary>
    public class ItemCost : ItemBase
    {
        [SerializeField] private ImageBind imgIcon;

        [SerializeField] private Text txtNumber;

        protected uint coin;

        protected float amount;

        public void Refresh(UIntPair pair)
        {
            this.coin = pair.x;

            this.amount = pair.y;

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