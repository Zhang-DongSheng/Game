namespace UnityEngine.SAM
{
    public class SAMBillboard : SAMBase
    {
        [SerializeField] private Axis axis;

        [SerializeField] private Circle circle;

        [SerializeField] private RectTransform viewPort;

        private Vector3Interval position;

        protected override void Transition(float step)
        {
            if (target == null) return;

            progress = curve.Evaluate(step);

            target.localPosition = position.Lerp(progress);
        }

        protected override void Compute()
        {
            status = Status.Compute;

            Vector2 space = new Vector2(viewPort.rect.width / 2f, viewPort.rect.height / 2f);

            Vector2 view = new Vector2(target.rect.width / 2f, target.rect.height / 2f);

            position.origin = view - space;

            position.destination = space - view;

            step = Config.ZERO;

            switch (axis)
            {
                case Axis.Horizontal:
                    position.origin.y = position.destination.y = 0;
                    break;
                case Axis.Vertical:
                    position.origin.x = position.destination.x = 0;
                    break;
                default:
                    break;
            }

            onBegin?.Invoke();

            status = Status.Transition;
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