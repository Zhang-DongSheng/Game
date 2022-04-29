using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemCost : ItemBase
    {
        [SerializeField] private Image imgIcon;

        [SerializeField] private Text txtNumber;

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
                SpriteHelper.SetSprite(imgIcon, name);
            }
        }

        private void RefreshNumberText(int value)
        {
            if (txtNumber != null)
            {
                txtNumber.text = string.Format("{0}", value);
            }
        }

        private void RefreshNumberColor(Color color)
        {
            if (txtNumber != null)
            {
                txtNumber.color = color;
            }
        }
    }
}