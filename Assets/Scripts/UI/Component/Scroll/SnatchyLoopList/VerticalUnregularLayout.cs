using System;

namespace UnityEngine.UI
{
    public sealed class VerticalUnregularLayout : UnregularLayout
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

                    size = expand ? new Vector2(view.x, method(i).y) : method(i)
                });
                position.y -= method(i).y;

                if (i < count - 1)
                {
                    position.y -= space.y;
                }
            }
            position.y -= padding.bottom;

            content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, -position.y);
        }

        protected override void Variable(bool force)
        {
            position = -content.anchoredPosition;

            position.y -= scroll.viewport.rect.size.y;

            rect.position = position;

            rect.size = scroll.viewport.rect.size;

            position.y += scroll.viewport.rect.size.y * 0.5f;

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
            point.y -= cell.size.y;

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