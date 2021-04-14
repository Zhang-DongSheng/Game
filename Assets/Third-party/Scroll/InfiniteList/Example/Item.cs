using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Example.Scroll.Infinite
{
    public class Item : InfiniteItem
    {
        protected override void Refresh()
        {
            base.Refresh();

            Text component = GetComponentInChildren<Text>();

            if (component != null)
            {
                component.text = Source.ToString();
            }
        }
    }
}