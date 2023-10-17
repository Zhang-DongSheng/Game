using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemNotice : ItemBase
    {
        [SerializeField] private CanvasGroup canvas;

        [SerializeField] private Vector2Interval position;

        [SerializeField] private float interval = 5f;

        [SerializeField] private Text tips;

        [SerializeField] private Style style;

        private float timer;

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
                            SetPosition(position.Lerp(1 - progress));
                        }
                    }
                    break;
            }
        }

        private void Complete()
        {
            SetActive(false);
        }

        public void Refresh(string content)
        {
            tips.text = content;

            timer = 0;

            canvas.alpha = 1;

            SetPosition(position.origination);

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