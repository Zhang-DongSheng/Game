using Data;
using Game.Resource;

public class GameConfig
{
    public const bool DEBUG = true;

    public const Language Lang = Language.Chinese;

    public const LoadType Load = LoadType.AssetBundle;

    public const string ServerURL_Login = "https://www.baidu.com/";

    public const string ServerURL_Game = "https://www.baidu.com/";

    public const string ServerURL_Resource = "https://branchapptest-1302051570.cos.ap-beijing.myqcloud.com/";

    public const string AssetBundle = "AssetBundle";

    public const string Resource = "Package";

    public const string History = "history.txt";

    public const string Manifest = "assets";
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