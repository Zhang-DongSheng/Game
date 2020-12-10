using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityEngine.SAM
{
    [RequireComponent(typeof(Graphic)), DisallowMultipleComponent]
    public class TouchAnimation : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private Transform target;

        [SerializeField] private Graphic graphic;

        [SerializeField] private TouchType touchType;

        [SerializeField] private SAMTransformInformation origin = new SAMTransformInformation();

        [SerializeField] private SAMTransformInformation destination = new SAMTransformInformation();

        [SerializeField] private Color originColor = Color.white;

        [SerializeField] private Color destinationColor = Color.white;

        [SerializeField] private AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

        [SerializeField, Range(0.1f, 100)] private float speed = 0.1f;

        [SerializeField] private float time = 1;

        [SerializeField] private bool useConfig = true;

        [SerializeField, Range(0, 1)] private float step;

        private float progress;

        private bool forward;

        private bool play;

        private bool graphicChanged;

        private void Awake()
        {
            if (target == null)
                target = transform;
            if (graphic == null)
                graphic = GetComponent<Graphic>();

            graphicChanged = originColor != destinationColor;

            speed = useConfig ? SAMConfig.SPEED : speed;
        }

        private void OnEnable()
        {
            Default();
        }

        private void Update()
        {
            if (play)
            {
                switch (touchType)
                {
                    case TouchType.Click:
                        ComputeClick();
                        break;
                    case TouchType.Press:
                    case TouchType.Drag:
                        ComputePress();
                        break;
                    case TouchType.Through:
                        ComputeThrough();
                        break;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    switch (touchType)
                    {
                        case TouchType.Drag:
                            forward = false;
                            break;
                    }
                }
            }
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                Transition(step);
            }
        }
#endif
        public void OnPointerEnter(PointerEventData eventData)
        {
            switch (touchType)
            {
                case TouchType.Through:
                    step = 0;
                    forward = true;
                    play = true;
                    break;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            switch (touchType)
            {
                case TouchType.Press:
                case TouchType.Drag:
                    step = 0;
                    forward = true;
                    play = true;
                    break;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (touchType)
            {
                case TouchType.Click:
                    step = 0;
                    forward = true;
                    play = true;
                    break;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            switch (touchType)
            {
                case TouchType.Press:
                    forward = false;
                    break;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            switch (touchType)
            {
                case TouchType.Through:
                    forward = false;
                    break;
            }
        }

        private void ComputeClick()
        {
            step += Time.deltaTime * speed * (forward ? 1 : -1);

            Transition(step);

            if (forward && step > time)
            {
                forward = false;
            }
            else if (!forward && 0 > step)
            {
                Stop();
            }
        }

        private void ComputePress()
        {
            step += Time.deltaTime * speed * (forward ? 1 : -1);

            step = step > 1 ? 1 : step;

            Transition(step);

            if (!forward && 0 > step)
            {
                Stop();
            }
        }

        private void ComputeThrough()
        {
            step += Time.deltaTime * speed * (forward ? 1 : -1);

            step = step > 1 ? 1 : step;

            Transition(step);

            if (!forward && 0 > step)
            {
                Stop();
            }
        }

        private void Transition(float step)
        {
            progress = curve.Evaluate(step);

            target.localPosition = Vector3.Lerp(origin.position, destination.position, progress);

            target.localEulerAngles = Vector3.Lerp(origin.rotation, destination.rotation, progress);

            target.localScale = Vector3.Lerp(origin.scale, destination.scale, progress);

            if (graphicChanged && graphic != null)
            {
                graphic.color = Color.Lerp(originColor, destinationColor, progress);
            }
        }

        private void Stop()
        {
            play = false;

            step = 0;

            Transition(step);
        }

        private void Default()
        {
            Transition(SAMConfig.ZERO);
        }

        enum TouchType
        {
            None,
            Click,
            Press,
            Drag,
            Through,
        }
    }
}