using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UnityEngine.SAM
{
    public class SAMSegment : MonoBehaviour
    {
        public Action onCompleted;

        [SerializeField] private List<SegmentAnimation> animations = new List<SegmentAnimation>();

        private SegmentAnimation current;

        private int index;

        private float step;

        private SAMStatus _status;
        protected SAMStatus Status
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
                    case SAMStatus.Compute:
                        step = SAMConfig.ZERO;
                        break;
                    case SAMStatus.Completed:
                        onCompleted?.Invoke();
                        break;
                }
            }
        }

        private void Awake()
        {
            StartUp();
        }

        private void Update()
        {
            if (Status == SAMStatus.Transition)
            {
                step += Time.deltaTime * current.speed;

                current.Transition(step);

                if (step > SAMConfig.ONE)
                {
                    Next();
                }
            }
        }

        private void Next()
        {
            Status = SAMStatus.Compute;

            if (animations.Count > ++index)
            {
                current = animations[index].Init();

                Status = SAMStatus.Transition;
            }
            else
            {
                Status = SAMStatus.Completed;

                Status = SAMStatus.Idel;
            }
        }

        public void StartUp()
        {
            index = -1;

            Next();
        }

        public void Stop()
        {
            Status = SAMStatus.Idel;
        }
    }

    [System.Serializable]
    public class SegmentAnimation
    {
        [SerializeField] private Transform target;

        [SerializeField] private Graphic graphic;

        [SerializeField] private SAMInformation origin = new SAMInformation();

        [SerializeField] private SAMInformation destination = new SAMInformation();

        [SerializeField] private AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

        [Range(0.1f, 100)] public float speed = SAMConfig.SPEED;

        private float progress;
         
        private bool graphicChanged;

        private Color color;

        public SegmentAnimation Init()
        {
            graphicChanged = origin.alpha != destination.alpha || origin.color != destination.color;

            return this;
        }

        public void Transition(float step)
        {
            if (target == null) return;

            progress = curve.Evaluate(step);

            target.localPosition = Vector3.Lerp(origin.position, destination.position, progress);

            target.localEulerAngles = Vector3.Lerp(origin.rotation, destination.rotation, progress);

            target.localScale = Vector3.Lerp(origin.scale, destination.scale, progress);

            if (graphicChanged && graphic != null)
            {
                color = Color.Lerp(origin.color, destination.color, progress);

                color.a = Mathf.Lerp(origin.alpha, destination.alpha, progress);

                graphic.color = color;
            }
        }
    }
}
