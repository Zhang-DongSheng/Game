namespace UnityEngine.UI
{
    public static class RectTransformUtils
    {
        public static bool Intersection(RectTransform transform, Vector2 position, Vector2 vector, out Vector2[] points)
        {
            points = new Vector2[4];

            int number = 0;
            // ио
            if (JunctionTop(transform.rect, position, vector, out Vector2 top))
            {
                points[number++] = top;
            }
            // вС
            if (JunctionLeft(transform.rect, position, vector, out Vector2 left))
            {
                points[number++] = left;
            }
            // ср
            if (JunctionRight(transform.rect, position, vector, out Vector2 right))
            {
                points[number++] = right;
            }
            // об
            if (JunctionBottom(transform.rect, position, vector, out Vector2 bottom))
            {
                points[number++] = bottom;
            }
            return number > 1;
        }

        public static void Scale(RectTransform transform, Vector4 space, Vector2 area)
        {
            float _width = space.y - space.x;

            float _height = space.w - space.z;

            float width = _width != 0 ? area.x / _width : 0;

            float height = _height != 0 ? area.y / _height : 0;

            float scale = width > height ? height : width;

            Scale(transform, space, scale);
        }

        public static void Scale(RectTransform transform, Vector4 space, float scale = 1f)
        {
            float width = space.y - space.x;

            float height = space.w - space.z;

            Vector2 position = new Vector2()
            {
                x = space.x + space.y,
                y = space.z + space.w,
            };
            transform.anchoredPosition = position * scale * -0.5f;

            transform.localScale = new Vector3(scale, scale, 1);

            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        public static Vector4 Space(RectTransform transform)
        {
            Vector4 space = new Vector4();

            foreach (var child in transform.GetComponentsInChildren<RectTransform>(true))
            {
                if (child != transform)
                {
                    space = Overflow(space, Child(child, Position(transform, child)));
                }
            }
            return space;
        }

        private static Vector4 Child(RectTransform transform, Vector2 position)
        {
            Vector2 size = new Vector2(transform.rect.width, transform.rect.height);

            transform.anchorMin = transform.anchorMax = Vector2.one * 0.5f;

            Vector4 space = new Vector4()
            {
                x = size.x * transform.pivot.x,
                y = size.x - size.x * transform.pivot.x,
                z = size.y * transform.pivot.y,
                w = size.y - size.y * transform.pivot.y,
            };

            space.x = position.x - space.x;

            space.y = position.x + space.y;

            space.z = position.y - space.z;

            space.w = position.y + space.w;

            return space;
        }

        private static Vector2 Position(RectTransform root, RectTransform child)
        {
            Vector2 position = child.anchoredPosition;

            Transform parent = child.parent;

            while (parent != null && parent != root)
            {
                position += parent.GetComponent<RectTransform>().anchoredPosition;

                parent = parent.parent;
            }
            return position;
        }

        private static Vector4 Overflow(Vector4 space, Vector4 overflow)
        {
            space.x = Mathf.Min(space.x, overflow.x);

            space.y = Mathf.Max(space.y, overflow.y);

            space.z = Mathf.Min(space.z, overflow.z);

            space.w = Mathf.Max(space.w, overflow.w);

            return space;
        }

        private static bool JunctionTop(Rect rect, Vector2 position, Vector2 vector, out Vector2 point)
        {
            point = new Vector2();

            if (vector.x == 0 && vector.y == 0)
            {

            }
            else if (vector.x == 0)
            {
                float height = rect.height * 0.5f;

                point = new Vector2(position.x, height);

                return true;
            }
            else if (vector.y == 0)
            {

            }
            else
            {
                float width = rect.width * 0.5f;

                float height = rect.height * 0.5f;

                var distance = height - position.y;

                float x = distance / vector.y * vector.x;

                x = position.x + x;

                if (x < width && x > -width)
                {
                    point = new Vector2(x, height);

                    return true;
                }
            }
            return false;
        }

        private static bool JunctionBottom(Rect rect, Vector2 position, Vector2 vector, out Vector2 point)
        {
            point = new Vector2();

            if (vector.x == 0 && vector.y == 0)
            {

            }
            else if (vector.x == 0)
            {
                float height = rect.height * 0.5f;

                point = new Vector2(position.x, -height);

                return true;
            }
            else if (vector.y == 0)
            {

            }
            else
            {
                float width = rect.width * 0.5f;

                float height = rect.height * 0.5f;

                var distance = position.y + height;

                float x = distance / vector.y * vector.x;

                x = position.x - x;

                if (x < width && x > -width)
                {
                    point = new Vector2(x, -height);

                    return true;
                }
            }
            return false;
        }

        private static bool JunctionLeft(Rect rect, Vector2 position, Vector2 vector, out Vector2 point)
        {
            point = new Vector2(0, 0);

            if (vector.x == 0 && vector.y == 0)
            {

            }
            else if (vector.x == 0)
            {

            }
            else if (vector.y == 0)
            {
                float width = rect.width * 0.5f;

                point = new Vector2(-width, position.y);

                return true;
            }
            else
            {
                float width = rect.width * 0.5f;

                float height = rect.height * 0.5f;

                var distance = position.x + width;

                float y = distance / vector.x * vector.y;

                y = position.y - y;

                if (y <= height && y >= -height)
                {
                    point = new Vector2(-width, y);

                    return true;
                }
            }
            return false;
        }

        private static bool JunctionRight(Rect rect, Vector2 position, Vector2 vector, out Vector2 point)
        {
            point = new Vector2(0, 0);

            if (vector.x == 0 && vector.y == 0)
            {

            }
            else if (vector.x == 0)
            {

            }
            else if (vector.y == 0)
            {
                float width = rect.width * 0.5f;

                point = new Vector2(width, position.y);

                return true;
            }
            else
            {
                float width = rect.width * 0.5f;

                float height = rect.height * 0.5f;

                var distance = width - position.x;

                float y = distance / vector.x * vector.y;

                y = position.y + y;

                if (y <= height && y >= -height)
                {
                    point = new Vector2(width, y);

                    return true;
                }
            }
            return false;
        }
    }
}