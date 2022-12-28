using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemCell : UnregularScrollItem
    {
        [SerializeField] private Image background;

        [SerializeField] private Text label;

        protected override void Refresh()
        {
            background.color.Rainbow(Index / 100f);

            //label.text = Source.ToString();
        }
    }
}