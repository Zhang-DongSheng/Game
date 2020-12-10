using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.SAM
{
    public class SAMGraphic : SAMBase
    {
        protected override void Renovate()
        {
            if (status == SAMStatus.Transition)
            {
                step += Time.deltaTime * speed;

                Transition(step);

                if (step >= SAMConfig.ONE)
                {
                    Completed();
                }
            }
        }

        protected override void Transition(float step)
        {
            
        }
    }
}
