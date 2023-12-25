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
    }
}