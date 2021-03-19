namespace UnityEngine.SAM
{
    public class SAMRotate : SAMBase
    {
        [SerializeField] private Circle circle;

        [SerializeField] private Vector3Interval rotation;

        [SerializeField] private bool around;

        protected override void Transition(float step)
        {
            if (target == null) return;

            switch (circle)
            {
                case Circle.Once:
                case Circle.PingPong:
                    {
                        progress = curve.Evaluate(step);

                        vector = rotation.Lerp(progress);

                        target.localEulerAngles = vector;
                    }
                    break;
                case Circle.Loop:
                    {
                        if (around)
                        {
                            target.RotateAround(rotation.destination, rotation.origin, Time.deltaTime * speed);
                        }
                        else
                        {
                            target.Rotate(rotation.origin * Time.deltaTime * speed);
                        }
                    }
                    break;
            }
        }

        protected override void Completed()
        {
            switch (circle)
            {
                case Circle.Once:
                    base.Completed();
                    break;
                case Circle.PingPong:
                    step = Config.ZERO; forward = !forward;
                    break;
                case Circle.Loop:
                    step = Config.ZERO;
                    break;
            }
        }
    }
}