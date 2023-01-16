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

            switch (cost.consume)
            {
                default:
                    break;
            }
        }

        private void RefreshCoinSprite(string name)
        {
            if (imgIcon != null)
            {
                imgIcon.SetSprite(name);
            }
        }

        private void RefreshNumberText(int value)
        {
            if (txtNumber != null)
            {
                txtNumber.SetText(value);
            }
        }
    }
}