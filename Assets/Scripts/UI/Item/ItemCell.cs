using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

namespace UnityEngine.UI
{
    public class ItemCell : UnregularScrollItem
    {
        [SerializeField] private Image background;

        [SerializeField] private Text label;

        protected override void Refresh()
        {
            background.color.Rainbow(Index / 100f);

            label.text = Source.ToString();
        }
    }
}