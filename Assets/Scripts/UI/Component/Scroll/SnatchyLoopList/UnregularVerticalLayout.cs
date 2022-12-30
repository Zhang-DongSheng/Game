using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace UnityEngine.UI
{
    public class UnregularVerticalLayout : UnregularLayout
    {
        protected override void Initialise(int count, Func<int, Vector2> size)
        {
            cells.Clear();

            position = Vector2.zero;

            position.y += padding.top;

            for (int i = 0; i < count; i++)
            {
                cells.Add(new UnregularScrollCell()
                {
                    index = i,

                    position = -position,

                    size = size(i)
                });

                position.y += size(i).y;

                position.y += space.y;
            }
            content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, position.y);
        }

        protected override void Variable()
        {
            position = -content.anchoredPosition;

            position.y -= scroll.viewport.rect.size.y;

            rect.position = position;

            rect.size = scroll.viewport.rect.size;

            position.y += scroll.viewport.rect.size.y * 0.5f;

            Calculate(position);

            Detection();

            Renovate();
        }

        protected override bool InSide(Rect rect, UnregularScrollCell cell)
        {
            Vector2 position = cell.position;

            if (rect.Contains(position))
            {
                return true;
            }

            position.y -= cell.size.y;

            return rect.Contains(position);
        }
    }
}
