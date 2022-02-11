using UnityEngine.Events;

namespace UnityEngine.SAM
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class SAMBase : MonoBehaviour
    {
        public UnityEvent onBegin, onCompleted;

        [SerializeField] protected AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

        [SerializeField] protected bool enable;

        [SerializeField] protected bool useConfig = true;

        [SerializeField, Range(0.1f, 100)] protected float speed = 1;

        [SerializeField, Range(0, 1)] protected float step;

        [SerializeField] protected RectTransform target;

        protected float progress;

        protected Vector3 vector;

        protected bool forward;

        protected Status status;

        private void Awake()
        {
            if (target == null)
            {
                target = GetComponent<RectTransform>();
            }
            speed = useConfig ? Config.Speed : speed;

            Init();
        }

        private void OnEnable()
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

        protected abstract void Init();

        protected abstract void Transition(float step);

        protected virtual void Renovate()
        {
            if (status == Status.Transition)
            {
                step += Time.deltaTime * speed;

                Transition(Format(forward, step));

                if (step >= Config.One)
                {
                    Completed();
                }
            }
        }

        protected virtual void Compute()
        {
            status = Status.Compute;

            step = Config.Zero;

            onBegin?.Invoke();

            status = Status.Transition;
        }

        protected virtual void Completed()
        {
            status = Status.Completed;

            onCompleted?.Invoke();

            status = Status.Idel;
        }

        protected virtual float Format(bool forward, float step)
        {
            if (forward)
            {
                return Mathf.Min(step, Config.One);
            }
            else
            {
                return Mathf.Max(Config.One - step, Config.Zero);
            }
        }

        protected virtual void SetActive(bool active)
        {
            if (target != null && target.gameObject.activeSelf != active)
            {
                target.gameObject.SetActive(active);
            }
        }

        protected virtual void SetActive(Component component, bool active)
        {
            if (component != null && component.gameObject.activeSelf != active)
            {
                component.gameObject.SetActive(active);
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
                    status = Status.Pause;
                    break;
                case Status.Pause:
                    status = Status.Transition;
                    break;
                default:
                    Debug.LogWarningFormat("Paause doesn't support current state :{0}", status);
                    break;
            }
        }

        public virtual void Stop()
        {
            status = Status.Idel;
        }

        public virtual void Default()
        {
            Transition(step = 0);
        }

        public bool Enable { get { return enable; } set { enable = value; } }
    }
}