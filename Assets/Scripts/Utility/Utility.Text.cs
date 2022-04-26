using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        public static class Text
        {
            public static string Color(Color color, string content)
            {
                return string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGB(color), content);
            }
        }
    }
}