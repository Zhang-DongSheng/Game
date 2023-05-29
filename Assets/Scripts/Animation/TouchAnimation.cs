using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// 触控动画
    /// </summary>
    [RequireComponent(typeof(Graphic)), DisallowMultipleComponent]
    public class TouchAnimation : RuntimeBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private Transform target;

        [SerializeField] private Graphic graphic;

        [SerializeField] private TouchType touchType;

        [SerializeField] private Vector2Interval position;

        [SerializeField] private Vector3Interval rotation;

        [SerializeField] private Vector3Interval scale = Vector3Interval.One;

        [SerializeField] private ColorInterval color = ColorInterval.White;

        [SerializeField] private AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

        [SerializeField, Range(0.1f, 100)] private float speed = 0.1f;

        [SerializeField] private float time = 1;

        [SerializeField, Range(0, 1)] private float step;

        private float progress;

        private bool forward;

        private bool play;

        protected override void OnAwake()
        {
            if (target == null)
                target = transform;
            if (graphic == null)
                graphic = GetComponent<Graphic>();
        }

        protected override void OnUpdate(float delta)
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

        private void OnEnable()
        {
            Default();
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
                    step = 0; forward = true;
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
                    step = 0; forward = true;
                    play = true;
                    break;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (touchType)
            {
                case TouchType.Click:
                    step = 0; forward = true;
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

            target.localPosition = position.Lerp(progress);

            target.localEulerAngles = rotation.Lerp(progress);

            target.localScale = scale.Lerp(progress);

            if (graphic != null)
            {
                graphic.color = color.Lerp(progress);
            }
        }

        private void Stop()
        {
            play = false;

            Default();
        }

        private void Default()
        {
            Transition(0);
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