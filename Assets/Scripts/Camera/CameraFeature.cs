using UnityEngine;

namespace Game
{
    public class CameraFeature : ItemBase
    {
        private const float ZERO = 0;

        [SerializeField] private GameObject platform;

        [SerializeField] private Transform _camera;

        [SerializeField] private Vector3 home;

        [SerializeField] private AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

        [SerializeField, Range(0.1f, 10)] private float speedFollow = 0.1f;

        [SerializeField, Range(0.1f, 10)] private float speedFeature = 0.2f;

        [SerializeField] private float height = 1;

        [SerializeField] private float distance = 1;

        private Vector3 origin, destination, position, rotation;

        private Vector3 offset;

        private float angle;

        private float speed;

        private float step, progress;

        private FeatureStatus _status;
        private FeatureStatus Status
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
                    case FeatureStatus.Idle:
                        break;
                    case FeatureStatus.Follow:
                        speed = speedFollow;
                        step = ZERO;
                        break;
                    case FeatureStatus.Feature:
                        speed = speedFeature;
                        step = ZERO;
                        break;
                    case FeatureStatus.End:
                        break;
                }
            }
        }

        protected override void OnAwake()
        {
            if (platform == null)
                platform = gameObject;
        }

        private void Start()
        {
            Status = FeatureStatus.Idle;

            SetActive(platform, false);
        }

        protected override void OnLateUpdate(float delta)
        {
            switch (Status)
            {
                case FeatureStatus.Follow:
                    Follow();
                    break;
                case FeatureStatus.Feature:
                    Feature();
                    break;
                default:
                    break;
            }
        }

        private void Follow()
        {
            step += Time.deltaTime * speed;

            progress = curve.Evaluate(step);

            position = Vector3.Lerp(origin, destination, progress);

            rotation = Quaternion.LookRotation(position + offset, destination).eulerAngles;

            rotation.z = ZERO;

            _camera.eulerAngles = rotation;

            platform.transform.position = position;

            if (step >= 1)
            {
                Status = FeatureStatus.Feature;
            }
        }

        private void Feature()
        {
            step += Time.deltaTime * speed;

            progress = step;

            rotation = Mathf.Lerp(0, angle, progress) * Vector3.up;

            platform.transform.eulerAngles = rotation;

            if (step >= 1)
            {
                Status = FeatureStatus.End;
            }
        }

        private void Ready(Vector3 position, Vector3 rotation)
        {
            Status = FeatureStatus.Ready;

            origin = home;

            destination = position;

            this.angle = rotation.y > 180 ? rotation.y - 360 : rotation.y;

            Vector3 angle = Quaternion.LookRotation(home, position).eulerAngles;

            offset = Vector3.zero;

            offset.x += distance * Mathf.Sin(angle.y * Mathf.Deg2Rad);

            offset.y += height;

            offset.z += distance * Mathf.Cos(angle.y * Mathf.Deg2Rad);

            _camera.localPosition = offset;

            platform.transform.eulerAngles = Vector3.zero;

            SetActive(platform, true);

            Status = FeatureStatus.Follow;
        }

        public void StartUp(Transform target)
        {
            if (target == null) return;

            StartUp(target.position, target.rotation.eulerAngles);
        }

        public void StartUp(Vector3 position, Vector3 rotation)
        {
            Ready(position, rotation);
        }

        enum FeatureStatus
        {
            Idle,
            Ready,
            Follow,
            Feature,
            End,
        }
    }
}