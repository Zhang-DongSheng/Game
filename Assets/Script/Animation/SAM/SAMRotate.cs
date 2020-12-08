namespace UnityEngine.SAM
{
    public class SAMRotate : SAMBase
    {
        [SerializeField] private SAMCircle circle;

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

            vector = Vector3.Lerp(origin.rotation, destination.rotation, progress);

            target.localEulerAngles = vector;
        }

        protected override void Completed()
        {
            status = SAMStatus.Completed;

            onCompleted?.Invoke();
        }

        protected override void Compute()
        {
            status = SAMStatus.Compute;

            step = 0;

            onBegin?.Invoke();

            status = SAMStatus.Transition;
        }

        public override void Begin(bool forward)
        {
            Compute();
        }
    }
}