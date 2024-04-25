using UnityEngine;

namespace Game.Model
{
    [DisallowMultipleComponent]
    public class ModelPosition : ItemBase
    {
        [SerializeField] private Vector3 origination;

        [SerializeField] private Vector3 destination;

        [SerializeField] private float speed = 10f;

        [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

        private Vector3 position;

        private float progress;

        private float step;

        protected override void OnUpdate(float delta)
        {
            progress += delta * speed;

            progress = Mathf.Clamp01(progress);

            step = curve.Evaluate(progress);

            position = Vector3.Lerp(origination, destination, step);

            transform.localPosition = position;
        }
    }
}