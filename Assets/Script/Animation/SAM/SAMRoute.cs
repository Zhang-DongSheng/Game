using System.Collections.Generic;

namespace UnityEngine.SAM
{
    public class SAMRoute : SAMBase
    {
        [SerializeField] private Circle circle;

        [SerializeField] private List<Vector3> route = new List<Vector3>();

        private int index, count, next;

        private Vector3Interval position;

        protected override void Init() { }

        protected override void Transition(float step)
        {
            if (target == null) return;

            progress = curve.Evaluate(step);

            target.localPosition = position.Lerp(progress);
        }

        protected override void Compute()
        {
            if (route.Count == 0) return;

            status = Status.Compute;

            count = route.Count;

            index = forward ? 0 : count - 1;

            Position(forward, index);

            step = Config.Zero;

            onBegin?.Invoke();

            status = Status.Transition;
        }

        protected override void Completed()
        {
            step = Config.Zero;

            index = forward ? index + 1 : index - 1;

            switch (circle)
            {
                case Circle.Single:
                    if (Over(forward, index))
                    {
                        base.Completed(); return;
                    }
                    break;
                case Circle.Round:
                    if (Over(forward, index))
                    {
                        forward = !forward;
                    }
                    break;
                case Circle.Always:
                    if (forward)
                    {
                        index = count > index ? index : index %= count;
                    }
                    else if (index < 0)
                    {
                        index += count;
                    }
                    break;
            }
            Position(forward, index);
        }

        private void Position(bool forward, int index)
        {
            if (forward)
            {
                position.origin = route[index];

                next = count > ++index ? index : index % count;

                position.destination = route[next];
            }
            else
            {
                position.destination = route[index];

                next = --index < 0 ? index + count : index;

                position.origin = route[next];
            }
        }

        private bool Over(bool forward, int index)
        {
            if (forward)
            {
                if (index > count - 2)
                {
                    return true;
                }
            }
            else
            {
                if (index < 1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}