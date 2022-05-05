using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game
{
    public class RuntimeBehaviour : MonoSingleton<RuntimeBehaviour>
    {
        private event FunctionBySingle update;

        private event FunctionBySingle updateFixed;

        private event Function updateLate;

        private int count;

        private void Update()
        {
            if (update == null) return;

            update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (updateFixed == null) return;

            updateFixed(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            if (updateLate == null) return;

            updateLate();
        }

        public void Register(FunctionBySingle action)
        {
            if (update != null)
            {
                foreach (var function in update.GetInvocationList())
                {
                    if (function.Equals(action))
                    {
                        return;
                    }
                }
                update += action;
            }
            else
            {
                update = action;
            }
        }

        public void Unregister(FunctionBySingle action)
        {
            if (update != null)
            {
                update -= action;
            }
        }
    }
}