namespace UnityEngine.SAM
{
    public class SAMShake : SAMBase
    {
        [SerializeField] private float intensity = 0.3f;

        private float range;

        private Vector3 position;

        private Quaternion rotation;

        protected override void Awake()
        {
            base.Awake();

            position = target.localPosition;
            rotation = target.localRotation;
        }

        protected override void Transition(float step)
        {
            if (target == null) return;

            target.localPosition = position + Random.insideUnitSphere * range;
            target.localRotation = new Quaternion(
                rotation.x + Random.Range(-range, range) * 0.2f,
                rotation.y + Random.Range(-range, range) * 0.2f,
                rotation.z + Random.Range(-range, range) * 0.2f,
                rotation.w + Random.Range(-range, range) * 0.2f);
        }

        protected override void Completed()
        {
            status = Status.Completed;

            onCompleted?.Invoke();

            Default();
        }

        protected override void Compute()
        {
            status = Status.Compute;

            step = 0;

            forward = true;

            range = intensity;

            onBegin?.Invoke();

            status = Status.Transition;
        }

        public override void Default()
        {
            target.localPosition = position;
            target.localRotation = rotation;
        }
    }
}