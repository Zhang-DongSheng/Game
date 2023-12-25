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
                    return "基础";
                case 1:
                    return "声音";
                case 2:
                    return "多语言";
                case 3:
                    return "SDK";
                default:
                    return index.ToString();
            }
        }
    }
}