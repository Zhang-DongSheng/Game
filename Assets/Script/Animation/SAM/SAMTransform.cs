namespace UnityEngine.SAM
{
    public class SAMTransform : SAMBase
    {
        [SerializeField] private SAMTransformInformation origin, destination;

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
    }

    [System.Serializable]
    public class SAMTransformInformation
    {
        public Vector3 position = Vector3.zero;

        public Vector3 rotation = Vector3.zero;

        public Vector3 scale = Vector3.one;
    }
}