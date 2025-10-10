using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TimerType type;

        [SerializeField] private float interval;

        [SerializeField] private bool auto;

        public UnityEvent<int> onValueChanged;

        private int index;

        private float timer;

        private bool active;

        private void Awake()
        {
            if (auto)
            {
                active = true;
            }
        }

        private void Update()
        {
            if (active)
            {
                timer += Time.deltaTime;

                if (timer >= interval)
                {
                    timer = 0;

                    Execute();
                }
            }
        }

        private void Execute()
        {
            switch (type)
            {
                case TimerType.Once:
                    {
                        active = false;
                    }
                    break;
                case TimerType.Loop:
                    {
                        index++;
                    }
                    break;
                case TimerType.Countdown:
                    {
                        index--;

                        active = index > 0;
                    }
                    break;
            }
            onValueChanged?.Invoke(index);
        }

        public void Startup(int type, float time)
        {
            this.type = (TimerType)type;

            switch (this.type)
            {
                case TimerType.Once:
                    {
                        interval = time;
                    }
                    break;
                case TimerType.Loop:
                    {
                        interval = time;

                        index = 0;
                    }
                    break;
                case TimerType.Countdown:
                    {
                        interval = 1f;

                        index = (int)time;
                    }
                    break;
            }
            active = true;
        }

        enum TimerType
        {
            Once,
            Loop,
            Countdown,
        }
    }
}