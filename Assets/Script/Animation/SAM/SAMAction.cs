using System;

namespace UnityEngine.SAM
{
    public class SAMAction : SAMBase
    {
        public Action<float> callback;

        protected override void Renovate()
        {
            if (status == Status.Transition)
            {
                step += Time.deltaTime;

                Transition(Format(forward, step));

                if (step >= Config.ONE)
                {
                    Completed();
                }
            }
        }

        protected override void Transition(float step)
        {
            progress = curve.Evaluate(step);

            callback?.Invoke(progress);
        }
    }
}