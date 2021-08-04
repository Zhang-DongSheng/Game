using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class Player : MonoBehaviour
    {
        [SerializeField] protected Animator animator;

        protected void Awake()
        {
            if (animator == null)
                animator = GetComponent<Animator>();
        }

        public virtual void Play(string clip)
        {
            animator.Play(clip);
        }

        public void Trigger(string name)
        {
            animator.SetTrigger(name);
        }

        public void SetParameter(string name, bool value)
        {
            animator.SetBool(name, value);
        }

        public void SetParameter(string name, int value)
        {
            animator.SetInteger(name, value);
        }

        public void SetParameter(string name, float value)
        {
            animator.SetFloat(name, value);
        }
    }
}