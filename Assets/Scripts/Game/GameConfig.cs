using Data;
using UnityEngine;

namespace Game
{
    public class GameConfig
    {
        public const string ServerLoginURL = "https://www.baidu.com";

        public const string ServerGameURL = "https://www.baidu.com";

        public static AppMode AppMode
        {
            get
            {
#if APPMODE_DEV
                return AppMode.Developing;
#elif APPMODE_QA
                return AppMode.QA;
#elif APPMODE_REL
                return AppMode.Release;
#else
                return AppMode.Developing;
#endif
            }
        }

        public static string Version = Application.version;

        public static DataText.Language Language = DataText.Language.Chinese;
    }

    public enum AppMode
    {
        Developing,
        QA,
        Release
    }
}