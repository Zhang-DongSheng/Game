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

        protected Status status;

        protected virtual void Awake()
        {
            speed = useConfig ? Config.SPEED : speed;
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

        protected abstract void Transition(float step);

        protected virtual void Renovate()
        {
            if (status == Status.Transition)
            {
                step += Time.deltaTime * speed;

                Transition(Format(forward, step));

                if (step >= Config.ONE)
                {
                    Completed();
                }
            }
        }

        protected virtual void Compute()
        {
            status = Status.Compute;

            step = Config.ZERO;

            onBegin?.Invoke();

            status = Status.Transition;
        }

        protected virtual void Completed()
        {
            status = Status.Completed;

            onCompleted?.Invoke();

            status = Status.Idel;
        }

        protected virtual void SetActive(bool active)
        {
            if (gameObject != null && gameObject.activeSelf != active)
            {
                gameObject.SetActive(active);
            }
        }

        public virtual void Begin(bool forward)
        {
            this.forward = forward;

            Compute();
        }

        public virtual void Pause(bool pause)
        {
            switch (status)
            {
                case Status.Transition:
                case Status.Pause:
                    status = pause ? Status.Pause : Status.Transition;
                    break;
                default:
                    Debug.LogWarningFormat("Current status : {0} don't support pause!");
                    break;
            }
        }

        public virtual void Close()
        {
            status = Status.Idel;
        }

        public virtual void Default()
        {
            Transition(step = 0);
        }

        public virtual float Format(bool forward, float step)
        {
            if (forward)
            {
                return Mathf.Min(step, Config.ONE);
            }
            else
            {
                return Mathf.Max(Config.ONE - step, Config.ZERO);
            }
        }

        public bool Enable { get { return enable; } set { enable = value; } }
    }
}