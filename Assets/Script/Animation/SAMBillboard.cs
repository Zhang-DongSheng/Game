namespace UnityEngine.SAM
{
    public class SAMBillboard : MonoBehaviour
    {
        [SerializeField] private RectTransform viewPort;

        [SerializeField] private RectTransform content;

        [SerializeField] private AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

        [SerializeField, Range(0.1f, 100)] private float speed = 1;

        [SerializeField] private bool useConfig;

        private readonly SAMInformation origin = new SAMInformation();

        private readonly SAMInformation destination = new SAMInformation();

        private SAMStatus status = SAMStatus.Idel;

        private SAMDirection direction = SAMDirection.Forward;

        private float step = 0;

        private float progress;

        private void Awake()
        {
            speed = useConfig ? SAMConfig.SPEED : speed;
        }

        private void Update()
        {
            if (status == SAMStatus.Transition)
            {
                switch (direction)
                {
                    case SAMDirection.Forward:
                        step += Time.deltaTime * speed;
                        if (step >= SAMConfig.ONE)
                        {
                            direction = SAMDirection.Back;
                        }
                        break;
                    case SAMDirection.Back:
                        step -= Time.deltaTime * speed;
                        if (step <= SAMConfig.ZERO)
                        {
                            direction = SAMDirection.Forward;
                        }
                        break;
                }
                Transition(step);
            }
        }

        private void Compute()
        {
            status = SAMStatus.Compute;

            float space = viewPort.rect.height / 2f;

            float view = content.rect.height / 2f;

            origin.position.y = view - space;

            destination.position.y = space - view;

            status = SAMStatus.Transition;
        }

        private void Transition(float step)
        {
            if (content == null) return;

            progress = curve.Evaluate(step);

            content.localPosition = Vector3.Lerp(origin.position, destination.position, progress);

            content.localEulerAngles = Vector3.Lerp(origin.rotation, destination.rotation, progress);

            content.localScale = Vector3.Lerp(origin.scale, destination.scale, progress);
        }

        public void StartUp()
        {
            Compute();
        }
    }
}