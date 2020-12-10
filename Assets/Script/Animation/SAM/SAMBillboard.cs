namespace UnityEngine.SAM
{
    public class SAMBillboard : SAMBase
    {
        [SerializeField] private SAMAxis axis;

        [SerializeField] private SAMCircle circle;

        [SerializeField] private RectTransform viewPort;

        private Vector3 origin, destination;

        protected override void Renovate()
        {
            if (status == SAMStatus.Transition)
            {
                step += Time.deltaTime * speed;

                if (step >= SAMConfig.ONE)
                {
                    step = SAMConfig.ZERO;

                    forward = !forward;

                    Completed();
                }
                Transition(forward ? step : 1 - step);
            }
        }

        protected override void Transition(float step)
        {
            if (target == null) return;

            progress = curve.Evaluate(step);

            target.localPosition = Vector3.Lerp(origin, destination, progress);
        }

        protected override void Completed()
        {
            if (circle == SAMCircle.Once)
            {
                status = SAMStatus.Completed;

                onCompleted?.Invoke();
            }
        }

        protected override void Compute()
        {
            status = SAMStatus.Compute;

            Vector2 space = new Vector2(viewPort.rect.width / 2f, viewPort.rect.height / 2f);

            Vector2 view = new Vector2(target.rect.width / 2f, target.rect.height / 2f);

            origin = view - space;

            destination = space - view;

            step = SAMConfig.ZERO;

            switch (axis)
            {
                case SAMAxis.Horizontal:
                    origin.y = destination.y = 0;
                    break;
                case SAMAxis.Vertical:
                    origin.x = destination.x = 0;
                    break;
                default:
                    break;
            }

            onBegin?.Invoke();

            status = SAMStatus.Transition;
        }

        public override void Begin(bool forward)
        {
            if (axis == SAMAxis.None) return;

            this.forward = forward;

            Compute();
        }
    }
}