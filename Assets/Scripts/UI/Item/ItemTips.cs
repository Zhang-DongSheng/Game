using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemTips : ItemBase
    {
        [SerializeField] private CanvasGroup canvas;

        [SerializeField] private Text tips;

        [SerializeField, Range(0.1f, 10f)] private float interval = 3f;

        [SerializeField, Range(0.1f, 99f)] private float speed = 10f;

        [SerializeField] private Style style;

        private float timer;

        private Vector3 position;

        public bool Active { get; private set; }

        protected override void OnUpdate(float delta)
        {
            timer += delta;

            if (timer > interval)
            {
                Complete();
            }
            else
            {
                Transform((interval - timer) / interval);
            }
        }

        private void Transform(float progress)
        {
            switch (style)
            {
                case Style.None:
                    {

                    }
                    break;
                default:
                    {
                        //½¥Òþ
                        if ((style & Style.Alpha) != 0)
                        {
                            canvas.alpha = progress;
                        }
                        //Î»ÒÆ
                        if ((style & Style.Translate) != 0)
                        {
                            position.y += Time.deltaTime * speed;

                            SetPosition(position);
                        }
                    }
                    break;
            }
        }

        private void Complete()
        {
            Default(false);
        }

        private void Default(bool active)
        {
            timer = 0;

            canvas.alpha = 1;

            position = Vector3.zero;

            SetPosition(position);

            SetActive(active);

            Active = active;
        }

        public void Startup(string message)
        {
            tips.text = message;

            Default(true);

            SetActive(true);
        }
        [Flags]
        enum Style
        {
            None,
            Alpha,
            Translate,
        }
    }
}