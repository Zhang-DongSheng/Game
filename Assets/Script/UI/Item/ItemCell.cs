using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

namespace UnityEngine.UI
{
    public class ItemCell : UnregularLoopItem
    {
        [SerializeField] private Image background;

        [SerializeField] private Text label;

        protected override void Refresh()
        {
            background.color.Rainbow(Index / (float)100);

            label.text = string.Format("No.{0}", Index);

            Size = new Vector2(500, Random.Range(100, 300));
        }
    }
}