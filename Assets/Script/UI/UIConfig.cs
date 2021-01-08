using UnityEngine;

public class UIConfig
{
    public const float ResolutionWidth = 2160;

    public const float ResolutionHeight = 1080f;

    public static readonly float ResolutionRatio = ResolutionWidth / ResolutionHeight;

    public static readonly float ScreenHalfWidth = Screen.width / 2f;

    public static readonly float ScreenHalfHeight = Screen.height / 2f;

    public static readonly float ScreenWidthRatio = ResolutionWidth / Screen.width;

    public static readonly float ScreenHeightRatio = ResolutionHeight / Screen.height;

    public static readonly float ScreenRatio = Screen.width / Screen.height;
} 