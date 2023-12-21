using System;

namespace Game
{
    public class Timer
    {
        public Action<int> callback;

        public float interval;

        public bool loop;

        private int index;

        private bool active;

        private float timer;

        public void Start(float interval, Action callback, bool loop = false)
        {
            Start(interval, (_) => callback?.Invoke(), loop);
        }

        public void Start(float interval, Action<int> callback, bool loop = false)
        {
            this.timer = 0;

            this.interval = interval;

            this.callback = callback;

            this.loop = loop;

            this.index = 0;

            active = true;
        }

        public void Update(float delta)
        {
            if (active)
            {
                timer += delta;

                if (timer >= interval)
                {
                    Execute();
                }
            }
        }

        private void Execute()
        {
            callback?.Invoke(index++);

            if (loop)
            {
                timer = 0;
            }
            else
            {
                active = false;
            }
        }
    }
}