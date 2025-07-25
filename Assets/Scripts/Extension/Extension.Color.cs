using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        public static Color32 ToColor32(this Color color)
        {
            return new Color32(
                (byte)Mathf.Round(Mathf.Clamp01(color.r) * 255f),
                (byte)Mathf.Round(Mathf.Clamp01(color.g) * 255f),
                (byte)Mathf.Round(Mathf.Clamp01(color.b) * 255f),
                (byte)Mathf.Round(Mathf.Clamp01(color.a) * 255f));
        }
        /// <summary>
        /// Color32转Color
        /// </summary>
        public static Color ToColor(this Color32 color32)
        {
            return new Color(color32.r / 255f, color32.g / 255f, color32.b / 255f, color32.a / 255f);
        }
        /// <summary>
        /// 变亮
        /// </summary>
        public static Color Lighter(this Color color)
        {
            return new Color()
            {
                r = color.r + Light,
                g = color.g + Light,
                b = color.b + Light,
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
                r = color.r - Light,
                g = color.g - Light,
                b = color.b - Light,
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
        /// <summary>
        /// 彩虹色
        /// </summary>
        public static Color Rainbow(this Color color, float progress)
        {
            progress = Mathf.Clamp01(progress);

            int index = (int)(progress * 6);

            float value = progress * 6.0f - index;

            float invert = 1 - value;

            switch (index % 6)
            {
                case 0:
                    color.r = 1;
                    color.g = value;
                    color.b = 0;
                    break;
                case 1:
                    color.r = invert;
                    color.g = 1;
                    color.b = 0;
                    break;
                case 2:
                    color.r = 0;
                    color.g = 1;
                    color.b = value;
                    break;
                case 3:
                    color.r = 0;
                    color.g = invert;
                    color.b = 1;
                    break;
                case 4:
                    color.r = value;
                    color.g = 0;
                    color.b = 1;
                    break;
                case 5:
                    color.r = 1;
                    color.g = 0;
                    color.b = invert;
                    break;
            }
            return color;
        }

        public static Color Gray(this Color color)
        {
            var rgb = new Vector3(color.r, color.g, color.b);

            var gray = Vector3.Dot(rgb, GrayRatio);

            return new Color(gray, gray, gray, color.a);
        }
    }
}