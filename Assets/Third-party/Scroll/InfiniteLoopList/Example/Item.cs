using UnityEngine.UI;

namespace Example.Scroll.InfiniteLoop
{
    public class Item : InfiniteLoopItem
    {
        public Text txt_label;

        protected override void Refresh()
        {
            txt_label.text = Source.ToString();
        }
    }
}