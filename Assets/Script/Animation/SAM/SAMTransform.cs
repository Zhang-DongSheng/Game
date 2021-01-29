namespace UnityEngine.SAM
{
    public class SAMTransform : SAMBase
    {
        [SerializeField] private Vector3Interval position;

        [SerializeField] private Vector3Interval rotation;

        [SerializeField] private Vector3Interval scale;

        protected override void Renovate()
        {
            if (status == Status.Transition)
            {
                step += Time.deltaTime * speed;

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

            target.anchoredPosition = position.Lerp(progress);

            target.localEulerAngles = rotation.Lerp(progress);

            target.localScale = scale.Lerp(progress);
        }
    }
}