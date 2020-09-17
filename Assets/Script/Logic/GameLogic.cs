using System.Collections.Generic;
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

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                timer.transform.ClearChildren();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Dictionary<int, string> ss = new Dictionary<int, string>()
                {
                    { 1, "A"},
                    { 2,"B"},
                    { 3,"C"}
                };

                Debug.LogError(ss.Find(x => x == "B"));

                //Debug.LogError(ss.FindByIndex(2));
            }
        }
    }
}