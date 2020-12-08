using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityEngine.SAM
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class SAMBase : MonoBehaviour
    {
        public UnityEvent onBegin, onCompleted;

        [SerializeField] protected RectTransform target;

        [SerializeField] protected SAMInformation origin;

        [SerializeField] protected SAMInformation destination;

        [SerializeField] protected AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

        [SerializeField, Range(0.1f, 100)] protected float speed = 1;

        [SerializeField] protected bool useConfig;

        [SerializeField] protected bool enable;

        [SerializeField, Range(0, 1)] protected float step;

        protected float progress;

        protected Vector3 vector;

        protected SAMStatus status;

        protected virtual void Awake()
        {
            speed = useConfig ? SAMConfig.SPEED : speed;
        }

        protected virtual void OnEnable()
        {
            if (enable)
            {
                Begin(true);
            }
        }

        protected virtual void OnValidate()
        {
            if (!Application.isPlaying)
            {
                Transition(step);
            }
        }

        protected virtual void Update()
        {
            Renovate();
        }

        protected abstract void Renovate();

        protected abstract void Transition(float step);

        protected abstract void Completed();

        protected virtual void Compute() { }

        public virtual void Begin(bool forward)
        {

        }

        public virtual void Pause()
        {
            status = SAMStatus.Idel;
        }

        public virtual void Close()
        {
            status = SAMStatus.Idel;
        }

        public virtual void Default()
        {
            Transition(0);
        }

        protected virtual void SetActive(bool active)
        {
            if (gameObject != null && gameObject.activeSelf != active)
            {
                gameObject.SetActive(active);
            }
        }
    }

    [System.Serializable]
    public class SAMInformation
    {
        public Vector3 position = Vector3.zero;

        public Vector3 rotation = Vector3.zero;

        public Vector3 scale = Vector3.one;

        public Vector2 size = Vector2.zero;
    }
}