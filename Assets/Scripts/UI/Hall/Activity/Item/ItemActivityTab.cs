using Data;
using UnityEngine;

namespace Game.UI
{
    public class ItemActivityTab : ItemToggle
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
            var table = DataActivity.Get((uint)index);

            if (table != null)
            {
                return table.name;
            }
            return content;
        }
    }
}