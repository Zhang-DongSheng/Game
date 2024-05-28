using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public sealed class RuntimeManager : MonoSingleton<RuntimeManager>
    {
        private bool pause;

        private readonly WaitForEndOfFrame frame = new WaitForEndOfFrame();

        private readonly Dictionary<RuntimeEvent, Action<float>> events = new Dictionary<RuntimeEvent, Action<float>>();

        private void Awake()
        {
            foreach (var e in Enum.GetValues(typeof(RuntimeEvent)))
            {
                events.Add((RuntimeEvent)e, null);
            }
        }

        private void OnDestroy()
        {
            events.Clear();
        }

        private void FixedUpdate()
        {
            if (pause) return;

            if (events.TryGetValue(RuntimeEvent.FixedUpdate, out Action<float> handler))
            {
                handler?.Invoke(Time.fixedDeltaTime);
            }
        }

        private void Update()
        {
            if (pause) return;

            if (events.TryGetValue(RuntimeEvent.Update, out Action<float> handler))
            {
                handler?.Invoke(Time.deltaTime);
            }
        }

        private void LateUpdate()
        {
            if (pause) return;

            if (events.TryGetValue(RuntimeEvent.LateUpdate, out Action<float> handler))
            {
                handler?.Invoke(Time.deltaTime);
            }
        }

        private IEnumerator Execute(Action action, YieldInstruction yield)
        {
            yield return yield;

            action?.Invoke();
        }

        public void Register(RuntimeEvent key, Action<float> value)
        {
            if (value is null) return;

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

        public void Unregister(RuntimeEvent key, Action<float> value)
        {
            if (value is null) return;

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

        public void Invoke(Action action, YieldInstruction yield)
        {
            StartCoroutine(Execute(action, yield));
        }

        public bool Pause
        {
            get
            {
                return pause;
            }
            set
            {
                pause = value;
            }
        }
    }
}