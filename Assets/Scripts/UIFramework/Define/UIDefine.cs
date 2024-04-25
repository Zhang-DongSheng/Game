using UnityEngine;

namespace Game.UI
{
    public static class UIDefine
    {
        public const float ResolutionWidth = 2160;

        public const float ResolutionHeight = 1080f;

        public const float MinusOne = -1;

        public const float Zero = 0;

        public const string Json = "";

        public const string Prefab = "Package/Prefab/UI/Panel";

        public static readonly float ResolutionRatio = ResolutionWidth / ResolutionHeight;

        public static readonly float ScreenHalfWidth = Screen.width / 2f;

        public static readonly float ScreenHalfHeight = Screen.height / 2f;

        public static readonly float ScreenWidthRatio = ResolutionWidth / Screen.width;

        public static readonly float ScreenHeightRatio = ResolutionHeight / Screen.height;

        public static readonly float ScreenRatio = (float)Screen.width / Screen.height;
    }

    public enum Axis
    {
        Horizontal,
        Vertical,
    }

    public enum Direction
    {
        None,
        Horizontal,
        Vertical,
        Custom,
    }

    public enum Corner
    {
        TopLeft,
        TopRight,
        LowerLeft,
        LowerRight,
    }
}