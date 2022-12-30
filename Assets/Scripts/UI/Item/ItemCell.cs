using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemCell : UnregularItem
    {
        [SerializeField] private Image background;

        [SerializeField] private Text label;

        protected override void Refresh()
        {
            background.color = background.color.Rainbow(Index / 100f);

            label.text = Index.ToString();
        }
    }
}