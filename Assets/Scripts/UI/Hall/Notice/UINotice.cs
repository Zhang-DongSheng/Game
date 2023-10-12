using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UINotice : UIBase
    {
        [SerializeField] private Display display;

        [SerializeField] private RectTransform content;

        [SerializeField] private CanvasGroup canvas;

        [SerializeField] private Text notice;

        [SerializeField, Range(1, 50)] private float speed = 1f;

        [SerializeField, Range(0, 10)] private float duration;

        private float step;

        private float timer;

        private Status status;

        protected override void OnUpdate(float delta)
        {
            switch (status)
            {
                case Status.Update:
                    {
                        timer += delta;

                        if (timer > duration)
                        {
                            status = Status.Complete;
                        }
                        else
                        {
                            OnValueChanged();
                        }
                    }
                    break;
                case Status.Complete:
                    {
                        OnCompleted();
                    }
                    break;
            }
        }

        public override void Refresh(UIParameter paramter)
        {
            status = Status.Idle;

            notice.text = paramter.Get<string>("notice");

            timer = 0;

            content.anchoredPosition = Vector2.zero;

            canvas.alpha = step = 0;

            status = Status.Update;
        }

        private void OnValueChanged()
        {
            switch (display)
            {
                case Display.Popup:
                    {
                        content.Translate(Vector3.up * speed * Time.deltaTime);

                        canvas.alpha = 1f;
                    }
                    break;
                case Display.Fade:
                    {
                        step += Time.deltaTime * speed;

                        if (step > 1) step = 1;

                        canvas.alpha = step;
                    }
                    break;
            }
        }

        private void OnCompleted()
        {
            status = Status.Idle;

            OnClickClose();
        }

        enum Status
        {
            Idle,
            Update,
            Complete,
        }

        enum Display
        {
            None,
            Popup,
            Fade,
        }
    }
}