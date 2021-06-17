using System;

namespace UnityEngine.SAM
{
    public class SAMAction : SAMBase
    {
        public Action<float> action;

        protected override void Transition(float step)
        {
            progress = curve.Evaluate(step);

            action?.Invoke(progress);
        }
    }
}