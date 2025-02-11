using Game.Data;
using Game.SM;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class DialogSystemContent : ItemBase
    {
        [SerializeField] private SMTranslate _animation;

        [SerializeField] private TextMeshProUGUI _txtContent;

        private float timer, interval;

        private float speed = 6f;

        private int current, length;

        private string content;

        private bool active;

        private readonly List<char> chars = new List<char>();

        protected override void OnUpdate(float delta)
        {
            if (active)
            {
                timer += delta * speed;

                if (timer > interval)
                {
                    timer = 0;

                    current++;

                    RefreshContent();
                }
            }
        }

        private void RefreshContent()
        {
            active = current < length;

            var text = content;

            if (active)
            {
                text = content.Substring(0, current);
            }
            _txtContent.text = text;
        }

        public void Refresh(DialogInformation dialog)
        {
            this.content = dialog.content;

            length = content.Length;

            current = 0;

            timer = 0;

            interval = 1;

            RefreshContent();
        }

        public void Skip()
        {
            current = length;

            RefreshContent();
        }

        public void ShowOrHide(bool active)
        {
            _animation.Begin(active);
        }
    }
}
