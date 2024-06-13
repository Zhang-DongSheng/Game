using UnityEngine;

namespace Game.SM
{
    /// <summary>
    /// 缩放
    /// </summary>
    public class SMScale : SMBase
    {
        [SerializeField] private Vector3Interval interval = Vector3Interval.Default;

        private Vector3 scale;

        protected override void Initialize()
        {
        
        }

        protected override void Transition(float progress)
        {
            if (target == null) return;

            scale = interval.Lerp(progress);

            target.localScale = scale;
        }
    }
}