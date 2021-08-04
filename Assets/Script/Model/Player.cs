using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    [RequireComponent(typeof(Animator))]
    public class Player : MonoBehaviour
    {
        [SerializeField] protected Transform target;

        [SerializeField] protected Animator animator;

        [SerializeField] protected float speed;

        protected virtual void Awake()
        {
            if (target == null)
                target = transform;
            if (animator == null)
                animator = GetComponent<Animator>();
        }

        public virtual void Move(Vector2 vector)
        {

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