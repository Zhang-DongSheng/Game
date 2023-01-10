using Data;
using Game.Resource;
using UnityEngine;

namespace Game
{
    public class GameConfig
    {
        public static DataText.Language Language = DataText.Language.Chinese;

        public const string ServerURL_Login = "https://www.baidu.com";

        public const string ServerURL_Game = "https://www.baidu.com";

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
    }

    public enum AppMode
    {
        Developing,
        QA,
        Release
    }
}