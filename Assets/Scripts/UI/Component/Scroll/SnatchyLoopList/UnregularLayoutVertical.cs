using System;

namespace UnityEngine.UI
{
    public sealed class UnregularLayoutVertical : UnregularLayout
    {
        protected override void Initialise()
        {
            content.anchorMin = new Vector2(0, 1);

            content.anchorMax = new Vector2(1, 1);

            content.pivot = new Vector2(0.5f, 1);
        }

        protected override void Create(int count, Func<int, Vector2> method)
        {
            cells.Clear();

            position = Vector2.zero;

            switch (alignment)
            {
                case TextAlignment.Left:
                    position.x += padding.left;
                    break;
                case TextAlignment.Center:
                    position.x += padding.left - padding.right;
                    break;
                case TextAlignment.Right:
                    position.x -= padding.right;
                    break;
            }
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

        protected override void Format(UnregularItem item)
        {
            switch (alignment)
            {
                case TextAlignment.Left:
                    {
                        item.Init(0, 1);
                    }
                    break;
                case TextAlignment.Center:
                    {
                        item.Init(0.5f, 1);
                    }
                    break;
                case TextAlignment.Right:
                    {
                        item.Init(1, 1);
                    }
                    break;
            }
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

            if (point.y >= rect.y && point.y < rect.y + rect.height)
            {
                return true;
            }
            point.y -= cell.size.y;

            if (point.y >= rect.y && point.y < rect.y + rect.height)
            {
                return true;
            }
            return false;
        }

        protected override bool Mark(Vector2 position)
        {
            if (Mathf.Abs(position.y - mark) < 0.1f) return false;

            mark = position.y;

            return true;
        }
    }
}