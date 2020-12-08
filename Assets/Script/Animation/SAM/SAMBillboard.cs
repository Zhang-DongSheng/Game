namespace UnityEngine.SAM
{
    public class SAMBillboard : SAMBase
    {
        [SerializeField] private SAMAxis axis;

        [SerializeField] private SAMCircle circle;

        [SerializeField] private bool direction;

        [SerializeField] private RectTransform viewPort;

        private bool forward;

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

            target.localPosition = Vector3.Lerp(origin.position, destination.position, progress);

            target.localEulerAngles = Vector3.Lerp(origin.rotation, destination.rotation, progress);

            target.localScale = Vector3.Lerp(origin.scale, destination.scale, progress);
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

            origin.position = view - space;

            destination.position = space - view;

            step = SAMConfig.ZERO;

            forward = direction;

            switch (axis)
            {
                case SAMAxis.Horizontal:
                    origin.position.y = destination.position.y = 0;
                    break;
                case SAMAxis.Vertical:
                    origin.position.x = destination.position.x = 0;
                    break;
                default:
                    break;
            }

            onBegin?.Invoke();

            status = SAMStatus.Transition;
        }

        public override void Begin(bool forward)
        {
            switch (axis)
            {
                case SAMAxis.None:
                    return;
                default:
                    Compute();
                    break;
            }
        }
    }
}