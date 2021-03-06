﻿using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public static partial class Extension
    {
        public static void ToFront(this ScrollRect scroll)
        {
            if (scroll.horizontal)
            {
                scroll.normalizedPosition = new Vector2(1, 0);
            }
            else if (scroll.vertical)
            {
                scroll.normalizedPosition = new Vector2(0, 1);
            }
        }

        public static void ToBack(this ScrollRect scroll)
        {
            scroll.normalizedPosition = new Vector2(0, 0);
        }
    }
}