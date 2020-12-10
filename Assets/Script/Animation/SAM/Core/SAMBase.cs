using UnityEngine.Events;

namespace UnityEngine.SAM
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class SAMBase : MonoBehaviour
    {
        public UnityEvent onBegin, onCompleted;

        [SerializeField] protected RectTransform target;

        [SerializeField] protected AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

        [SerializeField, Range(0.1f, 100)] protected float speed = 1;

        [SerializeField] protected bool useConfig;

        [SerializeField] protected bool enable;

        [SerializeField, Range(0, 1)] protected float step;

        protected bool forward;

        protected float progress;

        protected Vector3 vector;

        protected SAMStatus status;

        protected virtual void Awake()
        {
            speed = useConfig ? SAMConfig.SPEED : speed;
        }

        protected virtual void OnEnable()
        {
            if (enable)
            {
                Begin(true);
            }
        }

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                Transition(step);
            }
        }

        private void Update()
        {
            Renovate();
        }

        protected abstract void Renovate();

        protected abstract void Transition(float step);

        protected virtual void Completed()
        {
            status = SAMStatus.Completed;

            onCompleted?.Invoke();

            status = SAMStatus.Idel;
        }

        protected virtual void Compute()
        {
            status = SAMStatus.Compute;

            step = SAMConfig.ZERO;

            onBegin?.Invoke();

            status = SAMStatus.Transition;
        }

        public virtual void Begin(bool forward)
        {
            this.forward = forward;

            Compute();
        }

        public virtual void Pause(bool pause)
        {
            status = pause ? SAMStatus.Idel : SAMStatus.Transition;
        }

        public virtual void Close()
        {
            status = SAMStatus.Idel;
        }

        public virtual void Default()
        {
            Transition(0);
        }

        protected virtual void SetActive(bool active)
        {
            if (gameObject != null && gameObject.activeSelf != active)
            {
                gameObject.SetActive(active);
            }
        }
    }
}