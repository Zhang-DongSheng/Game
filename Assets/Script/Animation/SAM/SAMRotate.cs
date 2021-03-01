namespace UnityEngine.SAM
{
    public class SAMRotate : SAMBase
    {
        [SerializeField] private Circle circle;

        [SerializeField] private Vector3Interval rotation;

        [SerializeField] private bool around;

        protected override void Renovate()
        {
            if (status == Status.Transition)
            {
                step += Time.deltaTime * speed;

                switch (circle)
                {
                    case Circle.Once:
                        Transition(Format(forward, step));
                        if (step >= Config.ONE)
                        {
                            Completed();
                        }
                        break;
                    case Circle.PingPong:
                        Transition(Format(forward, step));
                        if (step >= Config.ONE)
                        {
                            step = Config.ZERO; forward = !forward;
                        }
                        break;
                    case Circle.Loop:
                        if (around)
                        {
                            target.RotateAround(rotation.destination, rotation.origin, Time.deltaTime * speed);
                        }
                        else
                        {
                            target.Rotate(rotation.origin * Time.deltaTime * speed);
                        }
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