using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public sealed class RuntimeBehaviour : MonoSingleton<RuntimeBehaviour>
    {
        private event FunctionBySingle updates;

        private event FunctionBySingle fixeds;

        private readonly WaitForEndOfFrame EndOfFrame = new WaitForEndOfFrame();

        private readonly Dictionary<float, WaitForSeconds> Seconds = new Dictionary<float, WaitForSeconds>();

        private void Update()
        {
            if (updates == null) return;

            updates(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (fixeds == null) return;

            fixeds(Time.fixedDeltaTime);
        }

        private IEnumerator Excute(Action action, YieldInstruction yield)
        {
            yield return yield;

            action?.Invoke();
        }

        public void RegisterUpdate(FunctionBySingle action)
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

        public void RegisterFixedUpdate(FunctionBySingle action)
        {
            if (fixeds != null)
            {
                foreach (var function in fixeds.GetInvocationList())
                {
                    if (function.Equals(action))
                    {
                        return;
                    }
                }
                fixeds += action;
            }
            else
            {
                fixeds = action;
            }
        }

        public void UnregisterUpdate(FunctionBySingle action)
        {
            if (updates != null)
            {
                updates -= action;
            }
        }

        public void UnregisterFixedUpdate(FunctionBySingle action)
        {
            if (fixeds != null)
            {
                fixeds -= action;
            }
        }

        public void InvokeWaitForEndOfFrame(Action action)
        {
            Invoke(action, EndOfFrame);
        }

        public void InvokeWaitForSeconds(Action action, float seconds)
        {
            if (!Seconds.ContainsKey(seconds))
            {
                Seconds.Add(seconds, new WaitForSeconds(seconds));
            }
            Invoke(action, Seconds[seconds]);
        }

        public void Invoke(Action action, YieldInstruction yield)
        {
            StartCoroutine(Excute(action, yield));
        }
    }
}