using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RuntimeMonoBehaviour : MonoSingleton<RuntimeMonoBehaviour>
    {
        private readonly List<RuntimeBase> runtimes = new List<RuntimeBase>();

        private int count;

        private void Update()
        {
            count = runtimes.Count;

            for (int i = 0; i < count; i++)
            {
                if (runtimes[i] != null)
                {
                    runtimes[i].OnUpdate(Time.deltaTime);
                }
            }
        }

        private void FixedUpdate()
        {
            count = runtimes.Count;

            for (int i = 0; i < count; i++)
            {
                if (runtimes[i] != null)
                {
                    runtimes[i].OnFixUpdate(Time.fixedDeltaTime);
                }
            }
        }

        private void LateUpdate()
        {
            count = runtimes.Count;

            for (int i = 0; i < count; i++)
            {
                if (runtimes[i] != null)
                {
                    runtimes[i].OnLaterUpdate();
                }
            }
        }

        public void Register(RuntimeBase runtime)
        {
            if (runtimes.Contains(runtime))
            {
                Debuger.LogWarning(Author.Script, "can't register the same runtime!");
            }
            else
            {
                runtimes.Add(runtime);
            }
        }

        public void Unregister(RuntimeBase runtime)
        {
            if (runtimes.Contains(runtime))
            {
                runtimes.Remove(runtime);
            }
        }
    }
}