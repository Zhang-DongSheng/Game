using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemTips : ItemBase
    {
        [SerializeField] private CanvasGroup canvas;

        [SerializeField] private Text tips;

        [SerializeField, Range(0.1f, 10f)] private float interval = 3f;

        private float timer;

        public Action callback;

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
            canvas.alpha = progress;
        }

        private void Complete()
        {
            SetActive(false);

            timer = 0;

            callback?.Invoke();
        }

        public void Startup(string message)
        {
            tips.text = message;

            timer = 0;

            canvas.alpha = 1;

            SetActive(true);
        }
    }
}