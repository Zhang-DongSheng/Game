using UnityEngine;

namespace Game.SM
{
    /// <summary>
    /// 旋转
    /// </summary>
    public class SMRotate : SMBase
    {
        [SerializeField] private Circle circle;

        [SerializeField] private Vector3Interval rotation;

        [SerializeField] private bool around;

        protected override void Init() { }

        protected override void Transition(float step)
        {
            if (target == null) return;

            switch (circle)
            {
                case Circle.Always:
                    {
                        if (around)
                        {
                            target.RotateAround(rotation.destination, rotation.origination, Time.deltaTime * speed);
                        }
                        else
                        {
                            target.Rotate(rotation.origination * Time.deltaTime * speed);
                        }
                    }
                    break;
                default:
                    {
                        progress = curve.Evaluate(step);

                        vector = rotation.Lerp(progress);

                        target.localEulerAngles = vector;
                    }
                    break;
            }
        }

        protected override void Completed()
        {
            switch (circle)
            {
                case Circle.Single:
                    base.Completed();
                    break;
                default:
                    forward = !forward; step = Config.ZERO;
                    break;
            }
        }
    }
}