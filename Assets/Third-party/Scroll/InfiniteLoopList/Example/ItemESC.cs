using UnityEngine.UI;

namespace Example
{
    public class ItemESC : InfiniteLoopItem
    {
        public Text txt_label;

        protected override void Refresh()
        {
            txt_label.text = Source.ToString();
        }
    }
}