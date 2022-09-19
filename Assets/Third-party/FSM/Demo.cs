using Unity.VisualScripting;
using UnityEngine;

namespace FSM
{
    public class Demo : MonoBehaviour
    {
        StateMachine machine;

        public void Awake()
        {
            machine = new StateMachine("״̬��");
            //��Ϸ��ͣ״̬
            State gamePause = new State("��ͣ״̬");

            gamePause.onEnter += () => {
                Debug.Log("������Ϸ��ͣ״̬");
            };

            gamePause.onStay += () => {
                Debug.Log("����Ϸ��ͣ״̬�и���");
            };

            gamePause.onExit += () => {
                Debug.Log("�뿪��Ϸ��ͣ״̬");
            };

            //��Ϸ����״̬
            State gameStart = new State("����״̬");

            gameStart.onEnter += () => {
                Debug.Log("������Ϸ��ʼ״̬");
            };

            gameStart.onStay += () => {
                Debug.Log("����Ϸ��ʼ״̬�и���");
            };

            gameStart.onExit += () => {
                Debug.Log("�뿪��Ϸ��ʼ״̬");
            };

            //��Ϸ����״̬
            State gameEnd = new State("ֹͣ״̬");

            gameEnd.onEnter += () => {
                Debug.Log("������Ϸֹͣ״̬");
            };

            gameEnd.onStay += () => {
                Debug.Log("����Ϸֹͣ״̬�и���");
            };

            gameEnd.onExit += () => {
                Debug.Log("�뿪��Ϸֹͣ״̬");
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
