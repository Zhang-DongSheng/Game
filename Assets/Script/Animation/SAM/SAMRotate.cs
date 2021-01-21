namespace UnityEngine.SAM
{
    public class SAMRotate : SAMBase
    {
        [SerializeField] private Circle circle;

        [SerializeField] private Vector3Interval rotation;

        [SerializeField] private Vector3 eulers;

        protected override void Renovate()
        {
            if (status == Status.Transition)
            {
                switch (circle)
                {
                    case Circle.Once:
                        step += speed * Time.deltaTime;

                        Transition(step);

                        if (step >= Config.ONE)
                        {
                            Completed();
                        }
                        break;
                    case Circle.Loop:
                        target.Rotate(eulers * speed * Time.deltaTime);
                        break;
                }
            }
        }

        protected override void Transition(float step)
        {
            if (target == null) return;

            progress = curve.Evaluate(step);

            vector = rotation.Lerp(progress);

            target.localEulerAngles = vector;
        }
    }
}