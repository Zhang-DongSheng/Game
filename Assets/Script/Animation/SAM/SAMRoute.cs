using System.Collections.Generic;

namespace UnityEngine.SAM
{
    public class SAMRoute : SAMBase
    {
        [SerializeField] private Circle circle;

        [SerializeField] private List<Vector3> route = new List<Vector3>();

        private int index, next;

        private Vector3Interval position;

        protected override void Renovate()
        {
            if (status == Status.Transition)
            {
                step += Time.deltaTime * speed;

                Transition(step);

                if (step >= Config.ONE)
                {
                    step = Config.ZERO;

                    index = forward ? index + 1 : index - 1;

                    switch (circle)
                    {
                        case Circle.Single:
                            if (Finish(forward, index, route.Count))
                            {
                                Completed(); return;
                            }
                            break;
                        case Circle.Round:
                            if (Finish(forward, index, route.Count))
                            {
                                forward = !forward;
                            }
                            break;
                        case Circle.Always:
                            if (forward)
                            {
                                index %= route.Count;
                            }
                            else
                            {
                                if (index < 0)
                                {
                                    index += route.Count;
                                }
                            }
                            break;
                    }
                    Position(forward, index, route);
                }
            }
        }

        protected override void Transition(float step)
        {
            if (target == null) return;

            progress = curve.Evaluate(step);

            target.localPosition = position.Lerp(progress);
        }

        protected override void Compute()
        {
            if (route.Count == 0) return;

            index = forward ? 0 : route.Count - 1;

            Position(forward, index, route);

            base.Compute();
        }

        private void Position(bool forward, int index, List<Vector3> route)
        {
            position.origin = route[index];

            if (forward)
            {
                next = ++index % route.Count;
            }
            else
            {
                next = --index < 0 ? index + route.Count : index;
            }

            position.destination = route[next];
        }

        private bool Finish(bool forward, int index, int count)
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