using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// 矩形
        /// </summary>
        public static class Rectangle
        {
            public static void EditorDraw(Rect position, int count, ref Rect[] rects)
            {
                var handle = position.position;

                var width = position.width * 0.42f;

                var space = 20f;

                rects[0] = new Rect(position)
                {
                    x = position.x,
                    width = width,
                    height = position.height
                };
                handle.x += width;

                width = position.width - width - space * (count - 1);

                width /= count;

                for (int i = 0; i < count; i++)
                {
                    var index = i + 1;

                    rects[index] = new Rect(position)
                    {
                        x = handle.x + i * width + Mathf.Max(i, 0) * space,
                        width = width,
                        height = position.height
                    };
                }
            }
        }
    }
}