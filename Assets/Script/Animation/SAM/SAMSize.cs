namespace UnityEngine.SAM
{
    public class SAMSize : SAMBase
    {
        [SerializeField] private SAMAxis axis;

        [SerializeField] private Vector2 origin, destination;

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

            vector = Vector2.Lerp(origin, destination, progress);

            switch (axis)
            {
                case SAMAxis.Horizontal:
                    target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, vector.x);
                    break;
                case SAMAxis.Vertical:
                    target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, vector.y);
                    break;
                default:
                    target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, vector.x);
                    goto case SAMAxis.Vertical;
            }
        }
    }
}