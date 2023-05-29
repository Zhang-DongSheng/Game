using UnityEngine;

namespace Game
{
    public class Movement : ItemBase
    {
        [SerializeField] protected TRS TRS;

        [SerializeField] protected MotorPattern pattern;

        [SerializeField, Range(0.1f, 100)] protected float speed = 1;

        [SerializeField, Range(0f, 5)] protected float acceleratedSpeed = 0;

        [SerializeField] protected Bounds bounds = new Bounds(Vector3.zero, new Vector3(1000, 5, 1000));

        [SerializeField] protected Vector3 target;

        protected Vector3 current;

        protected Vector3 velocity;

        protected float acceleration;

        private void Start()
        {
            switch (TRS)
            {
                case TRS.Translate:
                    current = target = transform.position;
                    break;
                case TRS.Rotate:
                    current = target = transform.eulerAngles;
                    break;
                case TRS.Scale:
                    current = target = transform.localScale;
                    break;
            }
        }

        protected override void OnUpdate(float delta)
        {
            switch (pattern)
            {
                case MotorPattern.MoveTowards:
                    MoveTowards();
                    break;
                case MotorPattern.Lerp:
                    Lerp(delta);
                    break;
                case MotorPattern.Slerp:
                    Slerp(delta);
                    break;
                case MotorPattern.SmoothDamp:
                    SmoothDamp(delta);
                    break;
                case MotorPattern.Acceleration:
                    Acceleration(delta);
                    break;
            }
            CheckForBounds(); Display();
        }

        protected virtual void MoveTowards()
        {
            current = Vector3.MoveTowards(current, target, speed);
        }

        protected virtual void Lerp(float delta)
        {
            current = Vector3.Lerp(current, target, speed * delta);
        }

        protected virtual void Slerp(float delta)
        {
            current = Vector3.Slerp(current, target, speed * delta);
        }

        protected virtual void SmoothDamp(float delta)
        {
            current = Vector3.SmoothDamp(current, target, ref velocity, delta, speed);
        }

        protected virtual void Acceleration(float delta)
        {
            if (Vector3.Distance(current, target) < Mathf.Epsilon)
                acceleration = speed;
            else
            {
                acceleration += acceleratedSpeed * delta;
                current = Vector3.MoveTowards(current, target, acceleration * delta);
            }
        }

        protected virtual void CheckForBounds()
        {
            current = bounds.ClosestPoint(current);
        }

        protected virtual void Display()
        {
            switch (TRS)
            {
                case TRS.Translate:
                    transform.position = current;
                    break;
                case TRS.Rotate:
                    transform.rotation = Quaternion.Euler(current);
                    break;
                case TRS.Scale:
                    transform.localScale = current;
                    break;
            }
        }

        public Vector3 Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }

        public Vector3 Current
        {
            get
            {
                return current;
            }
        }
    }

    public enum MotorPattern
    {
        MoveTowards,
        Lerp,
        Slerp,
        SmoothDamp,
        Acceleration
    }

    public enum TRS
    {
        Translate,
        Rotate,
        Scale,
    }
}