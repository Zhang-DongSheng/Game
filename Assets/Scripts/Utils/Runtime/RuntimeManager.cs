using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public sealed class RuntimeManager : MonoSingleton<RuntimeManager>
    {
        private readonly Dictionary<RuntimeEvent, FunctionBySingle> events = new Dictionary<RuntimeEvent, FunctionBySingle>();

        private readonly Dictionary<float, WaitForSeconds> seconds = new Dictionary<float, WaitForSeconds>();

        private readonly WaitForEndOfFrame frame = new WaitForEndOfFrame();

        private void Awake()
        {
            foreach (var e in Enum.GetValues(typeof(RuntimeEvent)))
            {
                events.Add((RuntimeEvent)e, null);
            }
            Application.lowMemory += OnLowMemory;
        }

        private void OnDestroy()
        {
            Application.lowMemory -= OnLowMemory;
        }

        private void FixedUpdate()
        {
            if (events.TryGetValue(RuntimeEvent.FixedUpdate, out FunctionBySingle function))
            {
                function?.Invoke(Time.fixedDeltaTime);
            }
        }

        private void Update()
        {
            if (events.TryGetValue(RuntimeEvent.Update, out FunctionBySingle function))
            {
                function?.Invoke(Time.deltaTime);
            }
        }

        private void LateUpdate()
        {
            if (events.TryGetValue(RuntimeEvent.LateUpdate, out FunctionBySingle function))
            {
                function?.Invoke(Time.deltaTime);
            }
        }

        private void OnLowMemory()
        {
            if (events.TryGetValue(RuntimeEvent.LowMemory, out FunctionBySingle function))
            {
                function?.Invoke(0);
            }
        }

        private IEnumerator Execute(Action action, YieldInstruction yield)
        {
            yield return yield;

            action?.Invoke();
        }

        public void Register(RuntimeEvent key, FunctionBySingle value)
        {
            if (value is null)
            {
                return;
            }
            if (events.ContainsKey(key))
            {
                if (events[key] != null)
                {
                    foreach (var v in events[key].GetInvocationList())
                    {
                        if (v.Equals(value)) return;
                    }
                    events[key] += value;
                }
                else
                {
                    events[key] = value;
                }
            }
            else
            {
                events.Add(key, value);
            }
        }

        public void Unregister(RuntimeEvent key, FunctionBySingle value)
        {
            if (value is null)
            {
                return;
            }
            if (events.ContainsKey(key))
            {
                if (events[key] != null)
                {
                    events[key] -= value;
                }
            }
        }

        public void InvokeWaitForEndOfFrame(Action action)
        {
            Invoke(action, frame);
        }

        public void InvokeWaitForSeconds(Action action, float seconds)
        {
            if (!this.seconds.ContainsKey(seconds))
            {
                this.seconds.Add(seconds, new WaitForSeconds(seconds));
            }
            Invoke(action, this.seconds[seconds]);
        }

        public void Invoke(Action action, YieldInstruction yield)
        {
            StartCoroutine(Execute(action, yield));
        }
    }
}