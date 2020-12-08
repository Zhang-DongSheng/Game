namespace UnityEngine.SAM
{
    public class SAMShake : SAMBase
    {
        [SerializeField] private float intensity = 0.3f;

        private Vector3 position;

        private Quaternion rotation;

        private float range;

        protected override void Awake()
        {
            position = target.position;
            rotation = target.rotation;
        }

        protected override void Renovate()
        {
            if (status == SAMStatus.Transition)
            {
                step += speed * Time.deltaTime;

                Transition(1 - step);

                if (step >= SAMConfig.ONE)
                {
                    Completed();
                }
            }
        }

        protected override void Transition(float step)
        {
            if (target == null) return;

            target.position = position + Random.insideUnitSphere * range;
            target.rotation = new Quaternion(
                rotation.x + Random.Range(-range, range) * 0.2f,
                rotation.y + Random.Range(-range, range) * 0.2f,
                rotation.z + Random.Range(-range, range) * 0.2f,
                rotation.w + Random.Range(-range, range) * 0.2f);
        }

        protected override void Completed()
        {
            status = SAMStatus.Completed;

            onCompleted?.Invoke();

            Default();
        }

        protected override void Compute()
        {
            status = SAMStatus.Compute;

            step = 0;

            range = intensity;

            onBegin?.Invoke();

            status = SAMStatus.Transition;
        }

        public override void Begin(bool forward)
        {
            Compute();
        }

        public override void Default()
        {
            target.position = position;
            target.rotation = rotation;
        }
    }
}