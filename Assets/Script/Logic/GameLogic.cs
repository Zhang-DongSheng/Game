using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameLogic : MonoSingleton<GameLogic>
    {
        public TextCompontentTimer timer;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                timer.CountDown(Random.Range(30, 100));
            }
        }
    }
}