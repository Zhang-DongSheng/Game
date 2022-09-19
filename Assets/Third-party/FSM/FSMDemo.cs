using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VGBasic.FSM;

public class FSMDemo : MonoBehaviour {

	StateMachine mach;

    void OnEnable () {
		//状态机
		mach = new StateMachine ("状态机");
		//游戏暂停状态
		State gamePause = new State ("暂停状态");

		gamePause.OnStateEnter += (s) => {
            Debug.Log("进入游戏暂停状态");
        };

		gamePause.OnStateUpdate += () => {
            Debug.Log("在游戏暂停状态中更新");
        };

		gamePause.OnStateExit += (s) => {
            Debug.Log("离开游戏暂停状态");
        };

		//游戏启动状态
		State gameStart = new State ("启动状态");

		gameStart.OnStateEnter += (s) => {
            Debug.Log("进入游戏开始状态");
        };

		gameStart.OnStateUpdate += () => {
            Debug.Log("在游戏开始状态中更新");
        };

		gameStart.OnStateExit += (s) => {
            Debug.Log("离开游戏开始状态");
        };

		//添加参数
		ParameterManager.Instance.AddBool("游戏状态控制参数",true);

		//定义过渡
		gamePause.AddTransition (new Transition (gamePause,
			gameStart, () => {
				//过渡条件
				if(ParameterManager.Instance.GetBool("游戏状态控制参数") == true)
					return true;
				else
					return false;
		}));

		gameStart.AddTransition (new Transition (gameStart,
			gamePause, () => {
				//过渡条件
				if(ParameterManager.Instance.GetBool("游戏状态控制参数") == false)
					return true;
				else
					return false;
		}));

		//添加状态
		mach.AddState (gameStart);
		mach.AddState (gamePause);
		mach.MachineStart ();
    }

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
            ParameterManager.Instance.SetBool ("游戏状态控制参数", false);
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
            ParameterManager.Instance.SetBool ("游戏状态控制参数", true);
		}
	}
}