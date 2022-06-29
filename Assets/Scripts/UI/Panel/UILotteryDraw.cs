using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// ³é½±´ó×ªÅÌ
    /// </summary>
    public class UILotteryDraw : UIBase
    {
        [SerializeField] private Transform target;

        [SerializeField] private Button button;

        [SerializeField] private FloatInterval speed = new FloatInterval(30, 200, 6f);

        [SerializeField] private FloatInterval step = new FloatInterval(0, 1, 0.1f);

        [SerializeField, Range(1, 20)] private int circle = 3;

        [SerializeField, Range(1, 5)] private int count = 2;

        private FloatInterval display = new FloatInterval();

        private float euler;

        private int number;

        private DrawStatus status;

        private void Awake()
        {
            button.onClick.AddListener(OnClickDraw);
        }

        protected override void OnUpdate(float delta)
        {
            switch (status)
            {
                case DrawStatus.Idle:

                    break;
                case DrawStatus.Ready:
                    {
                        speed.current += speed.value * delta;

                        euler += speed.current * delta;

                        Rotate(euler);

                        if (speed.current > speed.destination)
                        {
                            speed.current = speed.destination;

                            status = DrawStatus.Rotate;
                        }
                    }
                    break;
                case DrawStatus.Rotate:
                    {
                        euler += speed.current * delta;

                        if (360 > euler) { }
                        else
                        {
                            euler %= 360; number++;
                        }
                        Rotate(euler);

                        if (number > circle && euler > display.origin)
                        {
                            status = DrawStatus.Display;
                        }
                    }
                    break;
                case DrawStatus.Display:
                    {
                        step.current += step.value * Time.deltaTime;

                        euler = Mathf.Lerp(display.origin + display.value * step.current, display.destination, step.current);

                        Rotate(euler);

                        if (step.current > step.destination)
                        {
                            status = DrawStatus.Complete;
                        }
                    }
                    break;
                case DrawStatus.Complete:
                    {
                        Complete();

                        status = DrawStatus.Idle;
                    }
                    break;
            }
        }

        private void Rotate(float value)
        {
            target.localEulerAngles = Vector3.forward * value;
        }

        private void Complete()
        {
            Debug.LogError("Success!" + display.origin);
        }

        private void Copmpute()
        {
            //sum wight, get in random value

            float value = Random.Range(0, 360);

            number = 0;

            step.current = step.origin;

            speed.current = speed.origin;

            display.origin = value;

            display.value = 360 * count;

            display.destination = display.origin + display.value;

            status = DrawStatus.Ready;
        }

        private void OnClickDraw()
        {
            Copmpute();
        }

        enum DrawStatus
        {
            Idle,
            Ready,
            Rotate,
            Display,
            Complete,
        }
        [System.Serializable]
        struct FloatInterval
        {
            public float origin;

            public float destination;

            public float value;

            [System.NonSerialized] public float current;

            public FloatInterval(float origin, float destination, float value)
            {
                this.origin = origin;

                this.destination = destination;

                this.value = value;

                this.current = origin;
            }
        }
    }
}