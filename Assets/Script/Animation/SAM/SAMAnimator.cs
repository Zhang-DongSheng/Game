using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.SAM
{
    public class SAMAnimator : SAMBase
    {
        [SerializeField] private Animator animator;

        private AnimatorStateInfo current;

        protected override void Renovate()
        {
            if (status == Status.Transition)
            {
                if (current.normalizedTime >= Config.ONE)
                {
                    Completed();
                }
            }
        }

        protected override void Transition(float step)
        {
            
        }

        protected override void Compute()
        {
            status = Status.Compute;

            current = animator.GetCurrentAnimatorStateInfo(0);

            animator.SetTrigger("Start");

            status = Status.Transition;
        }
    }
}
