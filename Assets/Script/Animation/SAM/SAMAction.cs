using System;

namespace UnityEngine.SAM
{
    public class SAMAction : SAMBase
    {
        public Action<float> callback;

        protected override void Transition(float step)
        {
            progress = curve.Evaluate(step);

            callback?.Invoke(progress);
        }
    }
}