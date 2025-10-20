using UnityEngine;

namespace Game.SM
{
    /// <summary>
    /// 旋转
    /// </summary>
    public class SMRotate : SMBase
    {
        [SerializeField] private Vector3Interval interval;

        [SerializeField] private bool around;

        private Vector3 rotation;

        protected override void Initialize()
        {
        
        }

        protected override void Transition(float progress)
        {
            if (target == null) return;

            switch (circle)
            {
                case Circle.Loop:
                    {
                        if (around)
                        {
                            target.RotateAround(interval.start, interval.end, Time.deltaTime * speed);
                        }
                        else
                        {
                            target.Rotate(interval.end * Time.deltaTime * speed);
                        }
                    }
                    break;
                default:
                    {
                        rotation = interval.Lerp(progress);

                        target.localEulerAngles = rotation;
                    }
                    break;
            }
        }
    }
}