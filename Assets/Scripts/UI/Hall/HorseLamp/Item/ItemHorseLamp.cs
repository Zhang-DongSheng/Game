using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemHorseLamp : ItemBase
    {
        [SerializeField] private RectTransform target;

        [SerializeField] private Text text;

        [SerializeField] private float duration = 5;

        [SerializeField] private float stay = 1;

        private Vector2Interval interval;

        private float width, height;

        private float stamp, timer;

        private float progress;

        private State state;

        private Vector2 position;

        protected override void OnUpdate(float delta)
        {
            timer += delta;

            switch (state)
            {
                case State.From:
                    {
                        if (timer > stamp)
                        {
                            Switching(State.Stay);
                        }
                        else
                        {
                            Translate(timer, stamp, interval);
                        }
                    }
                    break;
                case State.Stay:
                    {
                        if (timer > stamp)
                        {
                            Switching(State.To);
                        }
                        else
                        {
                            Translate(timer, stamp, interval);
                        }
                    }
                    break;
                case State.To:
                    {
                        if (timer > stamp)
                        {
                            Switching(State.Complete);
                        }
                        else
                        {
                            Translate(timer, stamp, interval);
                        }
                    }
                    break;
            }
        }

        private void Switching(State state)
        {
            this.state = state;

            timer = 0;

            switch (state)
            {
                case State.Ready:
                    {
                        Switching(State.From);

                        Translate(timer, stamp, interval);

                        SetActive(true);
                    }
                    break;
                case State.From:
                    {
                        stamp = Mathf.Clamp((duration - stay) * 0.5f, 0, duration);

                        interval.start = new Vector2(width, height);

                        interval.end = Vector2.zero;
                    }
                    break;
                case State.Stay:
                    {
                        stamp = stay;

                        interval.start = Vector2.zero;

                        interval.end = Vector2.zero;
                    }
                    break;
                case State.To:
                    {
                        stamp = Mathf.Clamp((duration - stay) * 0.5f, 0, duration);

                        interval.start = Vector2.zero;

                        interval.end = new Vector2(-width, height);
                    }
                    break;
                case State.Complete:
                    {
                        SetActive(false);

                        Switching(State.Idle);
                    }
                    break;
            }
        }

        private void Translate(float time, float stamp, Vector2Interval interval)
        {
            progress = time / stamp;

            position = interval.Lerp(progress);

            target.localPosition = position;
        }

        public void Refresh(string content)
        {
            text.text = content;

            width = target.rect.width + target.GetCanvasSize().x;

            width *= 0.5f;

            height = 0;

            Switching(State.Ready);
        }

        public float Next => Mathf.Clamp(duration - (duration - stay) * 0.5f, 0, duration);

        public float Duration => duration;

        enum State
        {
            Idle,
            Ready,
            From,
            Stay,
            To,
            Complete,
        }
    }
}