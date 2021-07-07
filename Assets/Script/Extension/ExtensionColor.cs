using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        private const float LIGHT_OFFSET = 0.0625f;
        /// <summary>
        /// ����
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
        /// �䰵
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
        /// ��ת
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
        /// �޸�͸����
        /// </summary>
        public static Color Alpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
        /// <summary>
        /// ���ƺ�ɫ
        /// </summary>
        public static bool ApproximatelyBlack(this Color color)
        {
            return color.r + color.g + color.b <= Mathf.Epsilon;
        }
        /// <summary>
        /// ���ư�ɫ
        /// </summary>
        public static bool ApproximatelyWhite(this Color color)
        {
            return color.r + color.g + color.b >= 1 - Mathf.Epsilon;
        }
        /// <summary>
        /// ƽ��ֵ
        /// </summary>
        public static float Brightness(this Color color)
        {
            return (color.r + color.g + color.b) / 3f;
        }
        /// <summary>
        /// �ʺ�ɫ
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
    }
}