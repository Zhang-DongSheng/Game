using UnityEngine;

namespace Game.Resource
{
    public static class ResourceConfig
    {
        public const string AssetBundle = "AssetBundle";

        public const string Package = "Package";

        public const string Record = "record";

        public const string Manifest = "dependencies";

        public static readonly string CloudResources = string.Format("{0}/{1}/{2}", Utility.Path.Project, AssetBundle, Platform);

        public static readonly string LocalResources = string.Format("{0}/{1}", Application.persistentDataPath, AssetBundle);

        public static LoadingType Loading = LoadingType.AssetDatabase;

        public static string Path(LoadingType type, string path)
        {
            switch (type)
            {
                case LoadingType.Resources:
                    return Utility.Path.GetPathWithoutExtension(path);
                case LoadingType.AssetBundle:
                    {
                        path = Utility.Path.GetPathWithoutExtension(path).ToLower();
                    }
                    return string.Format("{0}/{1}", LocalResources, path);
                case LoadingType.AssetDatabase:
                    return string.Format("Assets/{0}", path);
                default:
                    return path;
            }
        }

        public static string Platform
        {
            get
            {
#if UNITY_EDITOR && UNITY_STANDALONE
                return "window";
#elif UNITY_ANDROID
                return "android";
#elif UNITY_IOS
                return "ios";
#else
                return "unknow";
#endif
            }
        }
    }
}