using UnityEngine.Events;

namespace UnityEngine
{
    public sealed class TimerPro : MonoBehaviour
    {
        private const int COUNT = 2;

        public float interval = 1;

        public UnityEvent<float> onValueChanged;

        public UnityEvent onCompleted;

        private float[] timer = new float[COUNT + 2] { 0, 0, 0, 0 };

        private Status status;

        private void Update()
        {
            if (status == Status.Idle) return;

            for (int i = 0; i < COUNT; i++)
            {
                timer[i] += Time.deltaTime;
            }
            if (timer[0] > interval)
            {
                timer[0] = 0; OnValueChanged();
            }
            switch (status)
            {
                case Status.Remaining:
                    {
                        if (Time.time > timer[COUNT])
                        {
                            OnCompleted();
                        }
                    }
                    break;
            }
        }

        private void OnValueChanged()
        {
            switch (status)
            {
                case Status.Remaining:
                    {
                        onValueChanged?.Invoke(timer[COUNT] - Time.time);
                    }
                    break;
                case Status.CycleTime:
                    {
                        onValueChanged?.Invoke(timer[1]);
                    }
                    break;
                case Status.CycleNumber:
                    {
                        onValueChanged?.Invoke(timer[COUNT + 1]++);
                    }
                    break;
            }
        }

        private void OnCompleted()
        {
            onCompleted?.Invoke();

            status = Status.Idle;
        }

        public void Remaining(float duration, float interval = -1)
        {
            if (interval != -1)
            {
                this.interval = interval;
            }
            for (int i = 0; i < COUNT; i++)
            {
                timer[i] = 0;
            }
            timer[COUNT] = Time.time + duration;

            status = Status.Remaining;
        }

        public void Cycle(float interval = -1, bool number = true)
        {
            if (interval != -1)
            {
                this.interval = interval;
            }
            for (int i = 0; i < COUNT; i++)
            {
                timer[i] = 0;
            }
            timer[COUNT + 1] = 0;

            status = number ? Status.CycleNumber : Status.CycleTime;
        }

        public void Stop()
        {
            status = Status.Idle;
        }

        enum Status
        {
            Idle,
            Remaining,
            CycleTime,
            CycleNumber,
        }
    }
}