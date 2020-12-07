using System;

namespace UnityEngine.SAM
{
    public class SAMRotate : MonoBehaviour
    {
        [SerializeField] private RectTransform target;

        [SerializeField] private SAMCircle type;

        [SerializeField] private Vector3 origin;

        [SerializeField] private Vector3 destination;

        [SerializeField] private Vector3 eulers;

        [SerializeField] private AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

        [SerializeField, Range(0.1f, 20)] private float speed = 0.1f;

        [SerializeField] private bool useConfig = true;

        [SerializeField, Range(0, 1)] private float step;

        private Vector3 current;

        private float progress;

        private bool play;

        public Action callBack;

        private void Awake()
        {
            if (target == null)
                target = GetComponent<RectTransform>();
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
        private void Update()
        {
            if (play)
            {
                switch (type)
                {
                    case SAMCircle.Once:
                        step += speed * Time.deltaTime;

                        Transition(step);

                        if (step > 1)
                        {
                            Stop();
                        }
                        break;
                    case SAMCircle.Loop:
                        target.Rotate(eulers * speed * Time.deltaTime);
                        break;
                }
            }
        }

        private void Transition(float step)
        {
            progress = curve.Evaluate(step);

            current = Vector3.Lerp(origin, destination, progress);

            target.localEulerAngles = current;
        }

        private void Stop()
        {
            callBack?.Invoke();

            callBack = null;

            play = false; step = 0;
        }

        public void Show()
        {
            step = 0; play = true;
        }

        public void Default()
        {
            Transition(0);
        }
    }
}