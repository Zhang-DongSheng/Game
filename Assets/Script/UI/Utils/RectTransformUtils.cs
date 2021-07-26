namespace UnityEngine.UI
{
    public static class RectTransformUtils
    {
        public static void Scale(RectTransform rect, Vector4 space, Vector2 area)
        {
            float width = space.y - space.x;

            float height = space.w - space.z;

            float _width = width != 0 ? area.x / width : 0;

            float _height = height != 0 ? area.y / height : 0;

            float scale = _width > _height ? _height : _width;

            Vector2 position = new Vector2()
            {
                x = space.x + space.y,
                y = space.z + space.w,
            };
            rect.anchoredPosition = position * scale * -0.5f;

            rect.localScale = new Vector3(scale, scale, 1);

            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        public static Vector4 Space(RectTransform root)
        {
            Vector4 space = new Vector4();

            foreach (var child in root.GetComponentsInChildren<RectTransform>(true))
            {
                if (child != root)
                {
                    space = Overflow(space, Child(child, Position(root, child)));
                }
            }
            return space;
        }

        private static Vector4 Child(RectTransform child, Vector2 position)
        {
            Vector2 size = new Vector2(child.rect.width, child.rect.height);

            child.anchorMin = child.anchorMax = Vector2.one * 0.5f;

            Vector4 space = new Vector4()
            {
                x = size.x * child.pivot.x,
                y = size.x - size.x * child.pivot.x,
                z = size.y * child.pivot.y,
                w = size.y - size.y * child.pivot.y,
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
    }
}