using UnityEngine;
using UnityEngine.Events;

namespace Game.SM
{
    public abstract class SMBase : RuntimeBehaviour
    {
        public UnityEvent onBegin, onCompleted;

        [SerializeField] protected AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

        [SerializeField] protected bool enable;

        [SerializeField] protected bool useConfig = true;

        [SerializeField, Range(0.1f, 100)] protected float speed = 1;

        [SerializeField, Range(0, 1)] protected float step;

        [SerializeField] protected Transform target;

        protected float progress;

        protected Vector3 vector;

        protected bool forward;

        protected Status status;

        private void Start()
        {
            if (target == null)
            {
                target = GetComponent<Transform>();
            }
            // 使用配置数据
            if (useConfig)
            {
                speed = Config.SPEED;
            }
            Init();
        }

        private void OnEnable()
        {
            if (enable)
            {
                Begin();
            }
        }

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                Transition(step);
            }
        }

        protected override void OnUpdate(float delta)
        {
            Renovate(delta);
        }

        protected abstract void Init();

        protected abstract void Transition(float step);

        protected virtual void Renovate(float delta)
        {
            if (status == Status.Transition)
            {
                step += delta * speed;

                Transition(Format(forward, step));

                if (step >= Config.ONE)
                {
                    Completed();
                }
            }
        }

        protected virtual void Ready()
        {
            status = Status.Ready;

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

        protected virtual float Format(bool forward, float step)
        {
            if (forward)
            {
                return Mathf.Clamp01(step);
            }
            else
            {
                return Mathf.Clamp01(Config.ONE - step);
            }
        }

        public virtual void Begin(bool forward = true)
        {
            this.forward = forward;

            Ready();
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
                    Debuger.LogWarning(Author.UI, string.Format("Paause doesn't support current state :{0}", status));
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