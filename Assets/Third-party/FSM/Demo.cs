using Unity.VisualScripting;
using UnityEngine;

namespace FSM
{
    public class Demo : MonoBehaviour
    {
        StateMachine machine;

        public void Awake()
        {
            machine = new StateMachine("×´Ì¬»ú");
            //ÓÎÏ·ÔÝÍ£×´Ì¬
            State gamePause = new State("ÔÝÍ£×´Ì¬");

            gamePause.onEnter += () => {
                Debug.Log("½øÈëÓÎÏ·ÔÝÍ£×´Ì¬");
            };

            gamePause.onStay += () => {
                Debug.Log("ÔÚÓÎÏ·ÔÝÍ£×´Ì¬ÖÐ¸üÐÂ");
            };

            gamePause.onExit += () => {
                Debug.Log("Àë¿ªÓÎÏ·ÔÝÍ£×´Ì¬");
            };

            //ÓÎÏ·Æô¶¯×´Ì¬
            State gameStart = new State("Æô¶¯×´Ì¬");

            gameStart.onEnter += () => {
                Debug.Log("½øÈëÓÎÏ·¿ªÊ¼×´Ì¬");
            };

            gameStart.onStay += () => {
                Debug.Log("ÔÚÓÎÏ·¿ªÊ¼×´Ì¬ÖÐ¸üÐÂ");
            };

            gameStart.onExit += () => {
                Debug.Log("Àë¿ªÓÎÏ·¿ªÊ¼×´Ì¬");
            };

            //ÓÎÏ·Æô¶¯×´Ì¬
            State gameEnd = new State("Í£Ö¹×´Ì¬");

            gameEnd.onEnter += () => {
                Debug.Log("½øÈëÓÎÏ·Í£Ö¹×´Ì¬");
            };

            gameEnd.onStay += () => {
                Debug.Log("ÔÚÓÎÏ·Í£Ö¹×´Ì¬ÖÐ¸üÐÂ");
            };

            gameEnd.onExit += () => {
                Debug.Log("Àë¿ªÓÎÏ·Í£Ö¹×´Ì¬");
            };

            gameStart.Add(new Transition(gameStart, gamePause, () =>
            {
                if (machine.Parameter.GetBool("pause"))
                {
                    return true;
                }
                return false;
            }));
            gameStart.Add(new Transition(gameStart, gameEnd, () =>
            {
                if (machine.Parameter.GetBool("exit"))
                {
                    return true;
                }
                return false;
            }));

            gamePause.Add(new Transition(gamePause, gameStart, () =>
            {
                if (machine.Parameter.GetBool("pause"))
                {
                    return false;
                }
                return true;
            }));
            gamePause.Add(new Transition(gamePause, gameEnd, () =>
            {
                if (machine.Parameter.GetBool("exit"))
                {
                    return true;
                }
                return false;
            }));
            machine.Add(gameStart, gamePause, gameEnd);

            machine.Default = gameStart;

            machine.Startup();

        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                bool pause = machine.Parameter.GetBool("pause");

                pause = !pause;

                machine.Parameter.SetBool("pause", pause);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                bool exit = machine.Parameter.GetBool("exit");

                exit = !exit;

                machine.Parameter.SetBool("exit", exit);
            }
            machine.Update();
        }
    }
}
