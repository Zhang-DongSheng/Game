using Game.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameLogic : MonoSingleton<GameLogic>
    {
        private void Start()
        {
            UIManager.Instance.Open<UILogin>("");
        }
    }
}