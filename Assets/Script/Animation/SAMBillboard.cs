using System;

namespace UnityEngine.SAM
{
    public class SAMBillboard : MonoBehaviour
    {
        [SerializeField] private BillboardType type;

        [SerializeField] private RectTransform viewPort;

        [SerializeField] private RectTransform content;

        [SerializeField] private AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

        [SerializeField, Range(0.1f, 100)] private float speed = 1;

        [SerializeField] private bool useConfig;

        private readonly SAMInformation origin = new SAMInformation();

        private readonly SAMInformation destination = new SAMInformation();

        private SAMStatus _status = SAMStatus.Idel;

        private SAMDirection direction = SAMDirection.Forward;

        private float step = 0;

        private float progress;

        public Action onCompleted;

        private void Awake()
        {
            speed = useConfig ? SAMConfig.SPEED : speed;
        }

        private void Update()
        {
            if (Status == SAMStatus.Transition)
            {
                switch (direction)
                {
                    case SAMDirection.Forward:
                        step += Time.deltaTime * speed;
                        if (step >= SAMConfig.ONE)
                        {
                            step = SAMConfig.ONE;

                            direction = SAMDirection.Back;

                            Completed();
                        }
                        break;
                    case SAMDirection.Back:
                        step -= Time.deltaTime * speed;
                        if (step <= SAMConfig.ZERO)
                        {
                            step = SAMConfig.ZERO;
                            
                            direction = SAMDirection.Forward;

                            Completed();
                        }
                        break;
                }
                Transition(step);
            }
        }

        private SAMStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;

                switch (_status)
                {
                    case SAMStatus.Completed:
                        onCompleted?.Invoke();
                        _status = SAMStatus.Idel;
                        break;
                }
            }
        }

        private void Compute()
        {
            Status = SAMStatus.Compute;

            float space = viewPort.rect.height / 2f;

            float view = content.rect.height / 2f;

            origin.position.y = view - space;

            destination.position.y = space - view;

            switch (this.type)
            {
                case BillboardType.TopToBottom:
                    step = 1;
                    direction = SAMDirection.Back;
                    break;
                case BillboardType.BottomToTop:
                    step = 0;
                    direction = SAMDirection.Forward;
                    break;
                case BillboardType.Loop:
                    step = 0.5f;
                    direction = SAMDirection.Forward;
                    break;
            }

            Status = SAMStatus.Transition;
        }

        private void Transition(float step)
        {
            if (content == null) return;

            progress = curve.Evaluate(step);

            content.localPosition = Vector3.Lerp(origin.position, destination.position, progress);

            content.localEulerAngles = Vector3.Lerp(origin.rotation, destination.rotation, progress);

            content.localScale = Vector3.Lerp(origin.scale, destination.scale, progress);
        }

        private void Completed()
        {
            switch (this.type)
            {
                case BillboardType.Loop:
                    break;
                default:
                    Status = SAMStatus.Completed;
                    break;
            }
        }

        public void StartUp(int index = -1)
        {
            if (index < (int)BillboardType.Count && index > -1)
            {
                this.type = (BillboardType)index;
            }
            else if (index == -1) { }
            else
            {
                return;
            }

            switch (this.type)
            {
                case BillboardType.None:
                    return;
                default:
                    Compute();
                    break;
            }
        }

        enum BillboardType
        { 
            None,
            TopToBottom,
            BottomToTop,
            Loop,
            Count,
        }
    }
}