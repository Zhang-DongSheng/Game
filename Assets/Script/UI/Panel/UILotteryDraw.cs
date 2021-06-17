using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UILotteryDraw : UIBase
    {
        [SerializeField] private Transform target;

        [SerializeField] private Button btn_draw;

        [SerializeField] private FloatInterval speed = new FloatInterval(30, 200, 6f);

        [SerializeField] private FloatInterval step = new FloatInterval(0, 1, 0.1f);

        [SerializeField, Range(1, 20)] private int circle = 3;

        [SerializeField, Range(1, 5)] private int display = 2;

        private FloatInterval m_display = new FloatInterval();

        private float m_step, m_speed;

        private float m_euler;

        private int m_number;

        private DrawStatus m_status;

        private void Awake()
        {
            btn_draw.onClick.AddListener(OnClickDraw);
        }

        private void Update()
        {
            switch (m_status)
            {
                case DrawStatus.Idle:

                    break;
                case DrawStatus.Ready:
                    {
                        m_speed += speed.value * Time.deltaTime;

                        m_euler += m_speed * Time.deltaTime;

                        Rotate(m_euler);

                        if (m_speed > speed.destination)
                        {
                            m_speed = speed.destination;

                            m_status = DrawStatus.Rotate;
                        }
                    }
                    break;
                case DrawStatus.Rotate:
                    {
                        m_euler += m_speed * Time.deltaTime;

                        if (360 > m_euler) { }
                        else
                        {
                            m_euler %= 360; m_number++;
                        }
                        Rotate(m_euler);

                        if (m_number > circle && m_euler > m_display.origin)
                        {
                            m_status = DrawStatus.Display;
                        }
                    }
                    break;
                case DrawStatus.Display:
                    {
                        m_step += step.value * Time.deltaTime;

                        m_euler = Mathf.Lerp(m_display.origin + m_display.value * m_step, m_display.destination, m_step);

                        Rotate(m_euler);

                        if (m_step > step.destination)
                        {
                            m_status = DrawStatus.Complete;
                        }
                    }
                    break;
                case DrawStatus.Complete:
                    {
                        Complete();

                        m_status = DrawStatus.Idle;
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
            Debug.LogError("Success!");
        }

        private void Copmpute()
        {
            //sum wight, get in random value

            float value = Random.Range(0, 360);

            m_number = 0;

            m_step = step.origin;

            m_speed = speed.origin;

            m_display.origin = value;

            m_display.value = 360 * display;

            m_display.destination = m_display.origin + m_display.value;

            m_status = DrawStatus.Ready;
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

            public FloatInterval(float origin, float destination, float value)
            {
                this.origin = origin;

                this.destination = destination;

                this.value = value;
            }
        }
    }
}