using System;
using UnityEngine.Events;

namespace UnityEngine
{
    public sealed class Timer : MonoBehaviour
    {
        [SerializeField] private TimerMode mode;

        [SerializeField] private float interval = 1;

        public UnityEvent<float> onValueChanged;

        public UnityEvent onCompleted;

        private bool active = true;

        private int index;

        private float terminalTime;

        private readonly TimerInformation timer = new TimerInformation();

        private void Awake()
        {
            timer.action = OnValueChanged;
        }

        private void Update()
        {
            if (!active) return;

            timer.Update();

            switch (mode)
            {
                case TimerMode.Countdown:
                    {
                        if (Time.time > terminalTime)
                        {
                            OnCompleted();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void OnValueChanged()
        {
            switch (mode)
            {
                case TimerMode.Countdown:
                    {
                        onValueChanged?.Invoke(terminalTime - Time.time);
                    }
                    break;
                default:
                    {
                        onValueChanged?.Invoke(index++);
                    }
                    break;
            }
        }

        private void OnCompleted()
        {
            onCompleted?.Invoke();

            active = false;
        }

        public void Startup(float interval = -1, float duration = -1)
        {
            if (interval != -1)
            {
                this.interval = interval;
            }
            timer.interval = this.interval;

            timer.timer = 0;

            terminalTime = Time.time + duration;

            index = 0; active = true;
        }

        public void Stop()
        {
            active = false;
        }
    }

    public class TimerInformation
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

    public enum TimerMode
    {
        None,
        Loop,
        Countdown,
    }
}