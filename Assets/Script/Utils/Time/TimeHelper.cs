using UnityEngine.Events;

namespace UnityEngine
{
    public class TimeHelper : MonoBehaviour
    {
        enum TimeStatus
        {
            Idle,
            Update,
            Complete,
        }

        [SerializeField] private float interval = 1;

        public UnityEvent<float> onValueChanged;

        public UnityEvent onCompleted;

        private float timer, end;

        private TimeStatus status;

        private void Update()
        {
            if (status == TimeStatus.Update)
            {
                timer += Time.deltaTime;

                if (timer >= interval)
                {
                    timer = 0;

                    OnValueChanged(Time.time - end);
                }

                if (Time.time >= end)
                {
                    status = TimeStatus.Complete;

                    OnCompleted();
                }
            }
        }

        private void OnValueChanged(float value)
        {
            onValueChanged?.Invoke(value);
        }

        private void OnCompleted()
        {
            onCompleted?.Invoke();
        }

        public void Startup(float time)
        {
            status = TimeStatus.Idle;

            end = Time.time + time;

            timer = 0;

            OnValueChanged(time);

            status = TimeStatus.Update;
        }
    }
}