using System;

namespace UnityEngine.UI
{
    public class HorizontalUnregularLayout : UnregularLayout
    {
        protected override void Initialise(int count, Func<int, Vector2> method)
        {
            cells.Clear();

            position = Vector2.zero;

            position.x += padding.left;

            position.y -= padding.top;

            for (int i = 0; i < count; i++)
            {
                cells.Add(new UnregularCell()
                {
                    index = i,

                    position = position,

                    size = expand ? new Vector2(method(i).x, view.y) : method(i)
                });
                position.x += method(i).x;

                if (i < count - 1)
                {
                    position.x += space.x;
                }
            }
            position.x += padding.right;

            content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, position.x);
        }

        protected override void Variable(bool force)
        {
            position = -content.anchoredPosition;

            rect.position = position;

            rect.size = scroll.viewport.rect.size;

            position.x += scroll.viewport.rect.size.x * 0.5f;

            Calculate(position);

            Detection();

            Renovate(force);
        }

        protected override bool InSide(Rect rect, UnregularCell cell)
        {
            point = cell.position;

            if (rect.Contains(point))
            {
                return true;
            }
            point.x += cell.size.x;

            return rect.Contains(point);
        }

        protected override bool Mark(Vector2 position)
        {
            if (Mathf.Abs(position.y - mark) < 0.1f) return false;

            mark = position.y;

            return true;
        }
    }
}