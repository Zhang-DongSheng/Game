/*--------  常量定义  -----------*/

using UnityEngine;

namespace Game.Data
{
    public static class AssetPath
    {
        public static string Assets = Application.dataPath;

        public static string Project = Application.dataPath[..^7];

        public const string Package = "Package";

        public const string Resources = "Resources";

        public const string HotUpdate = "HotUpdate";

        public const string Data = "Package/Data";

        public const string Json = "Art/Excel";

        public const string UIScript = "Scripts/UI/Hall";

        public const string UIPrefab = "Package/Prefab/UI/Panel";
    }
}