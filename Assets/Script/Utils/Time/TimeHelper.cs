using UnityEngine.Events;

namespace UnityEngine
{
    public class TimeHelper : MonoBehaviour
    {
        enum TimeStatus
        {
            Idle,
            Pause,
            Update,
            Complete,
        }

        [SerializeField] private float interval;

        [SerializeField] private bool fixedUpdate;

        public UnityEvent<float> onValueChanged;

        public UnityEvent onCompleted;

        private float timer, countdown;

        private TimeStatus status;

        private void Start()
        {
            
        }

        private void Update()
        {
            if (fixedUpdate) return;

            Update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (!fixedUpdate) return;

            Update(Time.fixedDeltaTime);
        }

        private void Update(float delta)
        {
            if (status == TimeStatus.Update)
            {
                countdown -= delta;

                timer += delta;

                if (timer >= interval)
                {
                    timer = 0;

                    OnValueChanged(countdown);
                }

                if (countdown <= 0)
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

        public void Start(float time)
        {
            status = TimeStatus.Idle;

            countdown = time;

            status = TimeStatus.Update;
        }
    }
}