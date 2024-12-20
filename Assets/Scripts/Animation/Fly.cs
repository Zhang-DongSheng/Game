using System;
using UnityEngine;

namespace Game
{
    public class Fly : ItemBase
    {
        [SerializeField] private Transform target;

        [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

        [SerializeField, Range(0.1f, 10f)] private float speed = 1f;

        [SerializeField] private Vector3Interval position;

        private float progress;

        private float step;

        private bool active;

        public Action callback;

        protected override void OnUpdate(float delta)
        {
            if (active)
            {
                step += delta * speed;

                progress = curve.Evaluate(step);

                target.localPosition = position.Lerp(progress);

                if (step > 1)
                {
                    callback?.Invoke(); active = false;
                }
            }
        }
        [ContextMenu("Starup")]
        public void Startup()
        {
            step = 0; active = true;
        }
    }
}