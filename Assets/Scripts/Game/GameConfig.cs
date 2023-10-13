using Game.Resource;
using UnityEngine;

namespace Game
{
    public class GameConfig
    {
        public static GameMode Mode;

        public const string ServerLoginURL = "https://www.baidu.com";

        public const string ServerGameURL = "https://www.baidu.com";
    }

    public enum GameMode
    {
        Develop,
        QA,
        Release
    }
}