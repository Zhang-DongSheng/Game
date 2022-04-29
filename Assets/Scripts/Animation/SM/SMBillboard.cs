using UnityEngine;

namespace Game.SM
{
    /// <summary>
    /// 广告牌
    /// </summary>
    public class SMBillboard : SMBase
    {
        [SerializeField] private RectTransform port;

        [SerializeField] private Axis axis;

        [SerializeField] private Circle circle;

        private Vector3Interval position;

        private RectTransform rect;

        protected override void Init()
        {
            if (!target.TryGetComponent(out rect))
            {
                Debuger.LogWarning(Author.UI, "The RectTransform is missing!");
            }
        }

        protected override void Transition(float step)
        {
            if (rect == null) return;

            progress = curve.Evaluate(step);

            rect.anchoredPosition = position.Lerp(progress);
        }

        protected override void Compute()
        {
            status = Status.Compute;

            Vector2 space = new Vector2(port.rect.width / 2f, port.rect.height / 2f);

            Vector2 view = new Vector2(rect.rect.width / 2f, rect.rect.height / 2f);

            position.origination = view - space;

            position.destination = space - view;

            switch (axis)
            {
                case Axis.Horizontal:
                    position.origination.y = position.destination.y = rect.anchoredPosition.y;
                    break;
                case Axis.Vertical:
                    position.origination.x = position.destination.x = rect.anchoredPosition.x;
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