using System;

namespace UnityEngine.UI
{
    public class UnregularLayoutHorizontal : UnregularLayout
    {
        protected override void Initialise()
        {
            content.anchorMin = new Vector2(0, 0);

            content.anchorMax = new Vector2(0, 1);

            content.pivot = new Vector2(0, 0.5f);
        }

        protected override void Create(int count, Func<int, Vector2> method)
        {
            cells.Clear();

            position = Vector2.zero;

            switch (alignment)
            {
                case TextAlignment.Left:
                    position.y -= padding.top;
                    break;
                case TextAlignment.Center:
                    position.y -= padding.top - padding.bottom;
                    break;
                case TextAlignment.Right:
                    position.y += padding.bottom;
                    break;
            }
            position.x += padding.left;

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
                        item.Init(0, 0.5f);
                    }
                    break;
                case TextAlignment.Right:
                    {
                        item.Init(0, 0);
                    }
                    break;
            }
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

            if (point.x >= rect.x && point.x < rect.x + rect.width)
            {
                return true;
            }
            point.x += cell.size.x;

            if (point.x >= rect.x && point.x < rect.x + rect.width)
            {
                return true;
            }
            return false;
        }

        protected override bool Mark(Vector2 position)
        {
            if (Mathf.Abs(position.x - mark) < 0.1f) return false;

            mark = position.x;

            return true;
        }
    }
}