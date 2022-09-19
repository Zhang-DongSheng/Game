using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VGBasic.FSM
{
    public class StateUpdate : MonoBehaviour
    {
        //单例
        public static StateUpdate Instance;

        public float stateDeltaTime = -1;

        //更新事件
        public StateEmptyEventHandle updateEvent;

        private void Awake()
        {
            Instance = this;
        }

        IEnumerator Start()
        {
            while (true)
            {
                if (stateDeltaTime != -1)
                {
                    yield return new WaitForSeconds(stateDeltaTime);
                }
                else
                {
                    yield return new WaitForEndOfFrame();
                }
                if (updateEvent != null)
                {
                    updateEvent();
                }
            }
        }
    }
}
