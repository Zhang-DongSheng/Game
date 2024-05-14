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
        /// <summary>
        /// 横
        /// </summary>
        Horizontal,
        /// <summary>
        /// 竖
        /// </summary>
        Vertical,
    }

    public enum Direction
    {
        None,
        /// <summary>
        /// 横
        /// </summary>
        Horizontal,
        /// <summary>
        /// 竖
        /// </summary>
        Vertical,
        /// <summary>
        /// 自定义
        /// </summary>
        Custom,
    }

    public enum Corner
    {
        /// <summary>
        /// 左上
        /// </summary>
        TopLeft,
        /// <summary>
        /// 右上
        /// </summary>
        TopRight,
        /// <summary>
        /// 左下
        /// </summary>
        LowerLeft,
        /// <summary>
        /// 右下
        /// </summary>
        LowerRight,
    }

    public enum Status
    {
        /// <summary>
        /// 未完成
        /// </summary>
        Undone,
        /// <summary>
        /// 可领取
        /// </summary>
        Available,
        /// <summary>
        /// 已领取
        /// </summary>
        Claimed,
    }
}