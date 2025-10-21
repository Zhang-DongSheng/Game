/*--------  常量定义  -----------*/
using UnityEngine;

namespace Game.UI
{
    public static class Const
    {
        public const float ResolutionWidth = 2160;

        public const float ResolutionHeight = 1080f;

        public const float MinusOne = -1;

        public const float Half = 0.5f;

        public const float Zero = 0;

        public static readonly float ResolutionRatio = ResolutionWidth / ResolutionHeight;

        public static readonly float ScreenHalfWidth = Screen.width * Half;

        public static readonly float ScreenHalfHeight = Screen.height * Half;

        public static readonly float ScreenWidthRatio = ResolutionWidth / Screen.width;

        public static readonly float ScreenHeightRatio = ResolutionHeight / Screen.height;

        public static readonly float ScreenRatio = (float)Screen.width / Screen.height;
    }
    /// <summary>
    /// 轴
    /// </summary>
    public enum Axis
    {
        None = 0,
        /// <summary>
        /// 横
        /// </summary>
        Horizontal,
        /// <summary>
        /// 竖
        /// </summary>
        Vertical,
        /// <summary>
        /// 斜
        /// </summary>
        Custom,
    }
    /// <summary>
    /// 渐
    /// </summary>
    public enum Fade
    {
        None = 0,
        /// <summary>
        /// 显
        /// </summary>
        In,
        /// <summary>
        /// 隐
        /// </summary>
        Out,
    }
}