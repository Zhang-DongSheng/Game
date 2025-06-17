using System;
using UnityEditor;
using UnityEngine;

namespace Game.SM
{
    [ExecuteInEditMode]
    public abstract class SMBase : MonoBehaviour
    {
        public Action onBegin, onCompleted;

        [SerializeField] protected Transform target;

        [SerializeField] protected AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

        [SerializeField] protected Circle circle;

        [SerializeField, Range(0.1f, 100)] protected float speed = 6;

        [SerializeField, Range(0, 1)] protected float step = 0;

        [SerializeField] protected bool forward = true;

        [SerializeField] protected bool enable;

        protected float progress;

        protected Status status;

        private void Awake()
        {
            if (target == null)
            {
                target = GetComponent<Transform>();
            }
            Initialize();
        }

        private void OnEnable()
        {
            if (enable)
            {
                Begin();
            }
        }

        private void Update()
        {
            Renovate(Time.deltaTime);
        }

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                progress = Progress(forward, step);

                Transition(progress);
            }
        }

        protected abstract void Initialize();

        protected abstract void Transition(float progress);

        protected virtual float Progress(bool forward, float step)
        {
            if (forward)
            {
                progress = Mathf.Clamp01(step);
            }
            else
            {
                progress = Mathf.Clamp01(1 - step);
            }
            return curve.Evaluate(progress);
        }

        protected virtual void Ready()
        {
            status = Status.Ready;

            step = 0;

            onBegin?.Invoke();

            status = Status.Transition;
        }

        protected virtual void Renovate(float delta)
        {
            if (status == Status.Transition)
            {
                step += delta * speed;

                progress = Progress(forward, step);

                Transition(progress);

                if(step < 1) return;

                step = 0;

                switch (circle)
                {
                    case Circle.Once:
                        {
                            Completed();
                        }
                        break;
                    case Circle.PingPong:
                        {
                            forward = !forward;
                        }
                        break;
                }
            }
        }

        protected virtual void Completed()
        {
            status = Status.Completed;

            onCompleted?.Invoke();

            status = Status.Idel;
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
                    Debuger.LogWarning(Author.UI, $"Paause doesn't support current state :{status}");
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
    }
}