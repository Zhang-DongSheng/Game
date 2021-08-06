using System;

namespace UnityEngine
{
    public class Clock
    {
        public Action action;

        public float timer = 0;

        public float interval = 1;

        public float speed = 1;

        public bool active = true;

        public void Update()
        {
            if (active)
            {
                timer += Time.deltaTime * speed;

                if (timer > interval)
                {
                    timer = 0;

                    action?.Invoke();
                }
            }
        }
    }
}