using System;

namespace UnityEngine.SAM
{
    [RequireComponent(typeof(RectTransform)), DisallowMultipleComponent]
    public class SAMTransform : MonoBehaviour
    {
        [SerializeField] private RectTransform target;

        [SerializeField] private CanvasGroup canvas;

        [SerializeField] private SAMInformation origin = new SAMInformation();

        [SerializeField] private SAMInformation destination = new SAMInformation();

        [SerializeField] private AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

        [SerializeField, Range(0.1f, 20)] private float speed = 0.1f;

        [SerializeField] private bool useConfig = true;

        public Action callBack;

        [SerializeField, Range(0, 1)] private float step;

        private float progress;

        private bool forward;

        private bool play;

        private void Awake()
        {
            if (target == null)
            {
                target = GetComponent<RectTransform>();
            }
            if (target != null && target is RectTransform)
            {
                if (origin.alpha != destination.alpha && canvas == null)
                {
                    canvas = target.gameObject.AddComponent<CanvasGroup>();
                }
            }
            speed = useConfig ? SAMConfig.SPEED : speed;
        }

        private void OnEnable()
        {
            Default(); Show();
        }

        private void OnDisable()
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
        private void FixedUpdate()
        {
            if (play)
            {
                step += speed * Time.deltaTime;

                Transition(forward ? step : 1 - step);

                if (step > 1)
                {
                    Stop();
                }
            }
        }

        public void Show(Action callBack = null)
        {
            this.callBack = callBack;

            SetActive(true);

            step = 0; forward = true; play = true;
        }

        public void Hide(Action callBack = null)
        {
            this.callBack = callBack;

            step = 0; forward = false; play = true;
        }

        private void Transition(float step)
        {
            progress = curve.Evaluate(step);

            target.anchoredPosition = Vector3.Lerp(origin.position, destination.position, progress);

            target.localEulerAngles = Vector3.Lerp(origin.rotation, destination.rotation, progress);

            target.localScale = Vector3.Lerp(origin.scale, destination.scale, progress);

            if (canvas != null) canvas.alpha = Mathf.Lerp(origin.alpha, destination.alpha, progress);
        }

        private void Stop()
        {
            SetActive(forward);

            callBack?.Invoke();

            callBack = null;

            play = false; step = 0;
        }

        private void SetActive(bool active)
        {
            if (gameObject != null && gameObject.activeSelf != active)
            {
                gameObject.SetActive(active);
            }
        }

        private void Default()
        {
            Transition(0);
        }

        public void ScaleAnimation(float origin = 0, float destination = 1)
        {
            this.origin.scale = origin * Vector3.one;
            this.destination.scale = destination * Vector3.one;
        }

        public void ShiftAnimation(Vector3 origin, Vector3 destination)
        {
            this.origin.position = origin;
            this.destination.position = destination;
        }

        public void FadeAnimation(float origin = 0, float destination = 1)
        {
            this.origin.alpha = origin;
            this.destination.alpha = destination;
        }

        public static SAMTransform AddCompontent(GameObject self, Transform target = null)
        {
            if (self == null) return null;

            SAMTransform animation = self.GetComponent<SAMTransform>();

            if (animation == null)
            {
                animation = self.AddComponent<SAMTransform>();
            }

            if (target == null)
            {
                target = self.transform.Find("Content");
            }
            animation.target = target != null ? target as RectTransform : self.transform as RectTransform;

            return animation;
        }
    }
}