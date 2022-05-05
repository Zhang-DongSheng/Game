using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class RuntimeBehaviour : MonoSingleton<RuntimeBehaviour>
    {
        private readonly WaitForEndOfFrame EndOfFrame = new WaitForEndOfFrame();

        private event FunctionBySingle updates;

        private void Update()
        {
            if (updates == null) return;

            updates(Time.deltaTime);
        }

        private IEnumerator Excute(Action action, YieldInstruction yield)
        {
            yield return yield;

            action?.Invoke();
        }

        public void Register(FunctionBySingle action)
        {
            if (updates != null)
            {
                foreach (var function in updates.GetInvocationList())
                {
                    if (function.Equals(action))
                    {
                        return;
                    }
                }
                updates += action;
            }
            else
            {
                updates = action;
            }
        }

        public void Unregister(FunctionBySingle action)
        {
            if (updates != null)
            {
                updates -= action;
            }
        }

        public void Invoke(Action action)
        {
            StartCoroutine(Excute(action, EndOfFrame));
        }

        public void Invoke(Action action, YieldInstruction yield)
        {
            StartCoroutine(Excute(action, yield));
        }
    }
}