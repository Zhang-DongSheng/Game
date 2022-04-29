using UnityEngine;

namespace Game.SM
{
    /// <summary>
    /// 缩放
    /// </summary>
    public class SMScale : SMBase
    {
        [SerializeField] private Vector3Interval scale;

        protected override void Init() { }

        protected override void Transition(float step)
        {
            if (target == null) return;

            progress = curve.Evaluate(step);

            vector = scale.Lerp(progress);

            target.localScale = vector;
        }
    }
}