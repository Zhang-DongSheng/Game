namespace UnityEngine.SAM
{
    public class SAMSize : SAMBase
    {
        [SerializeField] private Axis axis;

        [SerializeField] private Vector2Interval size;

        protected override void Renovate()
        {
            if (status == Status.Transition)
            {
                step += speed * Time.deltaTime;

                Transition(forward ? step : 1 - step);

                if (step >= Config.ONE)
                {
                    Completed();
                }
            }
        }

        protected override void Transition(float step)
        {
            if (target == null) return;

            progress = curve.Evaluate(step);

            vector = size.Lerp(progress);

            switch (axis)
            {
                case Axis.Horizontal:
                    target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, vector.x);
                    break;
                case Axis.Vertical:
                    target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, vector.y);
                    break;
                default:
                    target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, vector.x);
                    goto case Axis.Vertical;
            }
        }
    }
}