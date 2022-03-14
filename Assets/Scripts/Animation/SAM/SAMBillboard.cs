using UnityEngine;

namespace Game.SAM
{
    public class SAMBillboard : SAMBase
    {
        [SerializeField] private RectTransform viewPort;

        [SerializeField] private Axis axis;

        [SerializeField] private Circle circle;

        private Vector3Interval position;

        protected override void Init() { }

        protected override void Transition(float step)
        {
            if (target == null) return;

            progress = curve.Evaluate(step);

            target.anchoredPosition = position.Lerp(progress);
        }

        protected override void Compute()
        {
            status = Status.Compute;

            Vector2 space = new Vector2(viewPort.rect.width / 2f, viewPort.rect.height / 2f);

            Vector2 view = new Vector2(target.rect.width / 2f, target.rect.height / 2f);

            position.origination = view - space;

            position.destination = space - view;

            switch (axis)
            {
                case Axis.Horizontal:
                    position.origination.y = position.destination.y = target.anchoredPosition.y;
                    break;
                case Axis.Vertical:
                    position.origination.x = position.destination.x = target.anchoredPosition.x;
                    break;
                default:
                    break;
            }

            step = Config.Zero;

            onBegin?.Invoke();

            status = Status.Transition;
        }

        protected override void Completed()
        {
            switch (circle)
            {
                case Circle.Single:
                    base.Completed();
                    break;
                case Circle.Round:
                    forward = !forward; step = Config.Zero;
                    break;
                case Circle.Always:
                    step = Config.Zero;
                    break;
            }
        }
    }
}