using UnityEngine.Events;

namespace UnityEngine
{
    public sealed class Timer : MonoBehaviour
    {
        [SerializeField] private Mode mode;

        [SerializeField] private float interval = 1;

        public UnityEvent<float> onValueChanged;

        public UnityEvent onCompleted;

        private bool active = true;

        private int index;

        private float terminalTime;

        private readonly Clock timer = new Clock();

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
                case Mode.Countdown:
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
                case Mode.Countdown:
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

        enum Mode
        {
            None,
            Loop,
            Countdown,
        }
    }
}