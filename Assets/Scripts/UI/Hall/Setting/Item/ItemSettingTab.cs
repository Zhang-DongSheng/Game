using UnityEngine;

namespace Game.UI
{
    public class ItemSettingTab : ItemToggle
    {
        [SerializeField] private TextBind[] labels;

        public override void Refresh(ItemToggleKey key)
        {
            base.Refresh(key);

            content = Content(index);

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
                case 0:
                    return "10110";
                case 1:
                    return "10111";
                case 2:
                    return "10115";
                case 3:
                    return "10116";
                default:
                    return index.ToString();
            }
        }
    }
}