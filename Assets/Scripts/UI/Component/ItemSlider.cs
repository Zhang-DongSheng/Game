using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 进度条
    /// </summary>
    public class ItemSlider : ItemBase
    {
        private readonly Color alpha = new Color(0, 0, 0, 0);

        [SerializeField] private Image background;

        [SerializeField] private Image foreground;

        [SerializeField] private Color increase = Color.green;

        [SerializeField] private Color reduce = Color.red;

        [SerializeField] private float speed = 1;

        [SerializeField, Range(0, 1f)] private float value = 0;

        public UnityEvent<float> onValueChanged;

        public float origination, destination;

        private float step;

        private Status status;

        protected override void OnAwake()
        {
            if (background == null || foreground == null)
            {
                Debug.LogWarning("��Դ���ö�ʧ��");
            }
            else if (background.type != Image.Type.Filled)
            {
                Debug.LogWarning("����ͼƬ��ʽ����ȷ��");
            }
            else if (foreground.type != Image.Type.Filled)
            {
                Debug.LogWarning("ǰ��ͼƬ��ʽ����ȷ��");
            }
        }

        private void OnValidate()
        {
            
        }

        protected override void OnUpdate(float delta)
        {
            switch (status)
            {
                case Status.Increase:
                    {
                        step += delta * speed;

                        value = Mathf.Lerp(origination, destination, step);

                        foreground.fillAmount = value;

                        if (step >= 1)
                        {
                            status = Status.Complete;
                        }
                        OnValueChanged(value);
                    }
                    break;
                case Status.Reduce:
                    {
                        step += delta * speed;

                        value = Mathf.Lerp(origination, destination, step);

                        background.fillAmount = value;

                        if (step >= 1)
                        {
                            status = Status.Complete;
                        }
                        OnValueChanged(value);
                    }
                    break;
                case Status.Complete:
                    {
                        step = 0;

                        status = Status.Idle;

                        value = destination;

                        foreground.fillAmount = value;

                        background.fillAmount = value;

                        background.color = alpha;

                        OnValueChanged(value);
                    }
                    break;
            }
        }

        private void OnValueChanged(float value)
        {
            onValueChanged?.Invoke(value);
        }

        public void SetValue(float value)
        {
            if (this.value != value)
            {
                origination = this.value;

                destination = value;

                step = 0;

                status = origination > destination ? Status.Reduce : Status.Increase;

                switch (status)
                {
                    case Status.Increase:
                        {
                            foreground.fillAmount = origination;

                            background.fillAmount = destination;

                            background.color = increase;
                        }
                        break;
                    case Status.Reduce:
                        {
                            foreground.fillAmount = destination;

                            background.fillAmount = origination;

                            background.color = reduce;
                        }
                        break;
                }
            }
            else
            {
                SetValueImmediate(value);
            }
        }

        public void SetValueImmediate(float value)
        {
            status = Status.Idle;

            destination = origination = this.value = value;

            foreground.fillAmount = value;

            background.fillAmount = value;
        }

        enum Status
        {
            Idle,
            Increase,
            Reduce,
            Complete,
        }
    }
}