using Data;
using Game;
using Game.Resource;
using UnityEngine;

public class GameConfig
{
    public const bool DEBUG = true;

    public const Language Lang = Language.Chinese;

    public const LoadType Load = LoadType.AssetBundle;

    public const string ServerURL_Login = "https://www.baidu.com";

    public const string ServerURL_Game = "https://www.baidu.com";
#if UNITY_EDITOR
    public static string Server_Resource = string.Format("{0}/{1}/{2}", Utility.Path.Project, AssetBundle, BuildTarget);
#else
    public static string Server_Resource = "https://branchapptest-1302051570.cos.ap-beijing.myqcloud.com";
#endif
    public static string Local_Resource = string.Format("{0}/{1}", Application.persistentDataPath, AssetBundle);

    public const string AssetBundle = "AssetBundle";

    public const string Resource = "Package";

    public const string Record = "record";

    public const string Manifest = "dependencies";
#if UNITY_EDITOR && UNITY_STANDALONE
    public const string BuildTarget = "Window";
#elif UNITY_ANDROID
    public const string BuildTarget = "Android";
#elif UNITY_IOS
    public const string BuildTarget = "IOS";
#else
    public const string BuildTarget = "Unknow";
#endif
}