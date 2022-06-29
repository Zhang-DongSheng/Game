using System.Collections;
using UnityEngine;

namespace Game.Model
{
    public class ObjectBase : RuntimeBase
    {
        [SerializeField] AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

        [SerializeField, Range(0.1f, 10f)] private float speed = 1f;

        [SerializeField] private bool trigger;

        private float step;

        private Status status;

        private void Awake()
        {
            BoxCollider collider = GetComponent<BoxCollider>();

            if (collider != null)
            {
                collider.isTrigger = trigger;
            }
            status = Status.Alive;
        }

        protected override void OnUpdate(float delta)
        {
            switch (status)
            {
                case Status.Hit:
                    {
                        OnHit();

                        status = Status.Fall;
                    }
                    break;
                case Status.Fall:
                    {
                        step += Time.deltaTime * speed;

                        OnFall(curve.Evaluate(step));

                        if (step > 1)
                        {
                            status = Status.Die;
                        }
                    }
                    break;
                case Status.Die:
                    {
                        OnDie();

                        status = Status.Death;
                    }
                    break;
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (status != Status.Alive) return;

            OnTrigger(collider);

            status = Status.Hit;
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnTriggerEnter(collision.collider);
        }

        protected virtual void OnTrigger(Collider collider)
        {

        }

        protected virtual void OnHit()
        {

        }

        protected virtual void OnFall(float progress)
        {

        }

        protected virtual void OnDie()
        {

        }

        protected virtual IEnumerator DelayExecution()
        {
            yield return null;
        }

        enum Status
        {
            Alive,
            Hit,
            Fall,
            Die,
            Death,
        }
    }
}