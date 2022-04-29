using System;

namespace Game.SM
{
    public class SMAction : SMBase
    {
        public Action<float> action;

        protected override void Init() { }

        protected override void Transition(float step)
        {
            progress = curve.Evaluate(step);

            action?.Invoke(progress);
        }
    }
}