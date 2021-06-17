namespace UnityEngine.SAM
{
    public class SAMShake : SAMBase
    {
        [SerializeField] private float intensity = 0.3f;

        private Quaternion rotation;

        private Vector3 position;

        private float range;

        protected override void Awake()
        {
            base.Awake();

            position = target.localPosition;

            rotation = target.localRotation;
        }

        protected override void Transition(float step)
        {
            if (target == null) return;

            progress = curve.Evaluate(step);

            range = Mathf.Lerp(0, intensity, progress);

            target.localPosition = position + Random.insideUnitSphere * range;
            target.localRotation = new Quaternion(
                rotation.x + Random.Range(-range, range) * 0.2f,
                rotation.y + Random.Range(-range, range) * 0.2f,
                rotation.z + Random.Range(-range, range) * 0.2f,
                rotation.w + Random.Range(-range, range) * 0.2f);
        }

        protected override void Completed()
        {
            step = Config.ZERO;

            status = Status.Completed;

            target.localPosition = position;

            target.localRotation = rotation;

            onCompleted?.Invoke();

            status = Status.Idel;
        }
    }
}