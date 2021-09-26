using UnityEngine;

namespace Game.Model
{
    public class Stone : ObjectBase
    {
        [SerializeField] private Animator animator;

        [SerializeField] private bool destroy;

        protected override void OnHit()
        {
            animator.Play("Hit");
        }

        protected override void OnDie()
        {
            if (destroy)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}