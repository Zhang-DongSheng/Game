using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class UnregularHorizontalLayout : UnregularLayout
    {
        protected override void Initialise(int count, Func<int, Vector2> size)
        {
            cells.Clear();

            position = Vector2.zero;

            position.x += padding.left;

            for (int i = 0; i < count; i++)
            {
                cells.Add(new UnregularScrollCell()
                {
                    index = i,

                    position = position,

                    size = size(i)
                });

                position.x += size(i).x;

                position.x += space.x;
            }
            content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, position.x);
        }

        protected override void Variable()
        {
            position = -content.anchoredPosition;

            rect.position = position;

            rect.size = scroll.viewport.rect.size;

            position.x += scroll.viewport.rect.size.x * 0.5f;

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

            position.x += cell.size.x;

            return rect.Contains(position);
        }
    }
}
