using UnityEngine;

namespace Game.SM
{
    public class SMTransform : SMBase
    {
        [SerializeField] private Vector3Interval position;

        [SerializeField] private Vector3Interval rotation;

        [SerializeField] private Vector3Interval scale;

        protected override void Initialize()
        {
        
        }

        protected override void Transition(float progress)
        {
            if (target == null) return;

            target.localPosition = position.Lerp(progress);

            target.localEulerAngles = rotation.Lerp(progress);

            target.localScale = scale.Lerp(progress);
        }
    }
}