using System.Collections.Generic;

namespace UnityEngine.SAM
{
    public class SAMPath : SAMBase
    {
        [SerializeField] private List<Vector3> route = new List<Vector3>();

        [SerializeField] private Circle circle;

        private int index;

        private Vector3Interval position;

        protected override void Awake()
        {
            base.Awake();

            index = 0;

            Position(route);
        }

        protected override void Renovate()
        {
            if (route.Count == 0) return;

            if (status == Status.Transition)
            {
                step += speed * Time.deltaTime;

                Transition(step);

                if (step >= Config.ONE)
                {
                    step = 0; index++;

                    switch (circle)
                    {
                        case Circle.Once:
                            if (index + 2 > route.Count)
                            {
                                Completed(); return;
                            }
                            break;
                        case Circle.Loop:
                            if (index >= route.Count)
                            {
                                index = 0;
                            }
                            break;
                    }

                    Position(route, index);
                }
            }
        }

        protected override void Transition(float step)
        {
            if (target == null) return;

            progress = curve.Evaluate(step);

            target.localPosition = position.Lerp(progress);
        }

        private void Position(List<Vector3> route, int index = 0)
        {
            position.origin = route[index];

            position.destination = route[index + 1 >= route.Count ? 0 : index + 1];
        }
    }
}