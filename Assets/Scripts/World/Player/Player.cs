using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Game.Model
{
    [RequireComponent(typeof(Animator))]
    public abstract class Player : Entity
    {
        [SerializeField] protected Transform target;

        [SerializeField] protected Animator animator;

        protected override void OnAwake()
        {
            if (animator == null)
                animator = GetComponent<Animator>();
        }

        protected virtual void Play(string clip)
        {
            animator.Play(clip);
        }

        protected virtual void Trigger(string name)
        {
            animator.SetTrigger(name);
        }

        protected virtual void SetParameter(string name, bool value)
        {
            animator.SetBool(name, value);
        }

        protected virtual void SetParameter(string name, int value)
        {
            animator.SetInteger(name, value);
        }

        protected virtual void SetParameter(string name, float value)
        {
            animator.SetFloat(name, value);
        }

        public abstract void Born();

        public abstract void Die();

        public abstract void Idle();

        public abstract void Walk();

        public abstract void Run();

        public abstract void Jump();

        public abstract void Crouch();

        public abstract void Attack();

        public abstract void Damage();

        public abstract void ReleaseSkill(int index);

        public virtual void Move(Vector3 vector)
        {
            target.Translate(vector);
        }

        public virtual void Raotate(Quaternion rotation)
        {
            target.rotation = rotation;
        }
    }
}