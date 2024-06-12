using UnityEngine;

namespace Game.UI
{
    public class ItemShopTab : ItemToggle
    {
        [SerializeField] private TextBind[] labels;

        public override void Refresh(ItemToggleKey key)
        {
            base.Refresh(key);

            int count = labels.Length;

            for (int i = 0; i < count; i++)
            {
                labels[i].SetText(content);
            }
        }

        protected override string Content(int index)
        {
            switch (index)
            {
                case 101:
                    return "Package";
                case 102:
                    return "Sell";
                case 103:
                    return "Recharge";
            }
            return index.ToString();
        }
    }
}