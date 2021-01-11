using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig
{
    public const bool DEBUG = true;

    public const string LoginServerURL = "https://www.baidu.com/";

    public const string GameServerURL = "https://www.baidu.com/";

    public const string ResourceServerURL = "https://www.baidu.com/";

    public const string AssetBundlePath = "AssetBundle";

    public const string ArtPath = "Art";

    public const string History = "history.txt";

#if UNITY_EDITOR && UNITY_STANDALONE
    public static string BuildTarget = "Window";
#elif UNITY_ANDROID
    public static string BuildTarget = "Android";
#elif UNITY_IOS
    public static string BuildTarget = "IOS";
#else
    public static string BuildTarget = "Unknow";
#endif
}
