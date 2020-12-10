namespace UnityEngine.SAM
{
    public class SAMRotate : SAMBase
    {
        [SerializeField] private SAMCircle circle;

        [SerializeField] private Vector3 origin, destination;

        [SerializeField] private Vector3 eulers;

        protected override void Renovate()
        {
            if (status == SAMStatus.Transition)
            {
                switch (circle)
                {
                    case SAMCircle.Once:
                        step += speed * Time.deltaTime;

                        Transition(step);

                        if (step >= SAMConfig.ONE)
                        {
                            Completed();
                        }
                        break;
                    case SAMCircle.Loop:
                        target.Rotate(eulers * speed * Time.deltaTime);
                        break;
                }
            }
        }

        protected override void Transition(float step)
        {
            if (target == null) return;

            progress = curve.Evaluate(step);

            vector = Vector3.Lerp(origin, destination, progress);

            target.localEulerAngles = vector;
        }
    }
}