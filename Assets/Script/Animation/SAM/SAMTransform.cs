namespace UnityEngine.SAM
{
    public class SAMTransform : SAMBase
    {
        private bool forward;

        protected override void Renovate()
        {
            if (status == SAMStatus.Transition)
            {
                step += speed * Time.deltaTime;

                Transition(forward ? step : 1 - step);

                if (step >= SAMConfig.ONE)
                {
                    Completed();
                }
            }
        }

        protected override void Transition(float step)
        {
            if (target == null) return;

            progress = curve.Evaluate(step);

            target.anchoredPosition = Vector3.Lerp(origin.position, destination.position, progress);

            target.localEulerAngles = Vector3.Lerp(origin.rotation, destination.rotation, progress);

            target.localScale = Vector3.Lerp(origin.scale, destination.scale, progress);
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
            this.forward = forward;

            Compute();
        }
    }
}