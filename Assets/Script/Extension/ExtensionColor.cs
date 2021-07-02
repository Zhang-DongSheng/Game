using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        private const float LIGHT_OFFSET = 0.0625f;
        /// <summary>
        /// 变亮
        /// </summary>
        public static Color Lighter(this Color color)
        {
            return new Color()
            {
                r = color.r + LIGHT_OFFSET,
                g = color.g + LIGHT_OFFSET,
                b = color.b + LIGHT_OFFSET,
                a = color.a
            };
        }
        /// <summary>
        /// 变暗
        /// </summary>
        public static Color Darker(this Color color)
        {
            return new Color()
            {
                r = color.r - LIGHT_OFFSET,
                g = color.g - LIGHT_OFFSET,
                b = color.b - LIGHT_OFFSET,
                a = color.a
            };
        }
        /// <summary>
        /// 反转
        /// </summary>
        public static Color Invert(this Color color)
        {
            return new Color()
            {
                r = 1 - color.r,
                g = 1 - color.g,
                b = 1 - color.b,
                a = color.a
            };
        }
        /// <summary>
        /// 修改透明度
        /// </summary>
        public static Color Alpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
        /// <summary>
        /// 近似黑色
        /// </summary>
        public static bool ApproximatelyBlack(this Color color)
        {
            return color.r + color.g + color.b <= Mathf.Epsilon;
        }
        /// <summary>
        /// 近似白色
        /// </summary>
        public static bool ApproximatelyWhite(this Color color)
        {
            return color.r + color.g + color.b >= 1 - Mathf.Epsilon;
        }
        /// <summary>
        /// 平均值
        /// </summary>
        public static float Brightness(this Color color)
        {
            return (color.r + color.g + color.b) / 3f;
        }
    }
}