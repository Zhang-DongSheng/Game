using UnityEngine;

namespace Game
{
    public class GameConfig
    {
        public const string ServerLoginURL = "https://www.baidu.com";

        public const string ServerGameURL = "https://www.baidu.com";
    }

    public enum GameMode
    {
        Developing,
        QA,
        Release
    }
}