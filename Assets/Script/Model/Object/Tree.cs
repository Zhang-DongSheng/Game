using System.Collections;
using UnityEngine;

namespace Game.Model
{
    [RequireComponent(typeof(BoxCollider))]
    public class Tree : ObjectBase
    {
        [SerializeField] private HitType type;

        [SerializeField] private Transform tree;

        [SerializeField] private Transform root;

        [SerializeField] private Transform body;

        [SerializeField, Range(0, 20)] private float time = 1;

        private Vector3 vector, angle;

        private Vector3 origination, destination;

        private Quaternion rotation;

        private void Awake()
        {
            origination = tree.localEulerAngles;

            rotation = root.rotation;
        }

        protected override void OnTrigger(Collider collider)
        {
            switch (type)
            {
                case HitType.Transform:
                    {
                        vector = collider.transform.position - transform.position;
                    }
                    break;
                case HitType.Rigidbody:
                    {
                        Rigidbody rigidbody = collider.attachedRigidbody;

                        if (rigidbody != null)
                        {
                            vector = rigidbody.velocity;
                        }
                        else
                        {
                            goto case HitType.Transform;
                        }
                    }
                    break;
                case HitType.RigidbodyWithTransform:
                    {
                        Rigidbody rigidbody = collider.attachedRigidbody;

                        if (rigidbody != null)
                        {
                            Vector3 v1 = rigidbody.velocity;

                            Vector3 v2 = collider.transform.position - transform.position;

                            vector = v1.normalized + v2.normalized;
                        }
                        else
                        {
                            goto case HitType.Transform;
                        }
                    }
                    break;
                case HitType.Special:
                    {
                        vector = Vector3.forward;
                    }
                    break;
                default:
                    break;
            }

            angle.x = origination.x;

            angle.y = Vector2.SignedAngle(vector.Vector3To2(), Vector2.up);
        }

        protected override void OnHit()
        {
            destination = origination;

            destination.y = angle.y;

            tree.localEulerAngles = destination;

            destination.y = origination.y - angle.y;

            root.localEulerAngles = destination;

            body.localEulerAngles = destination;

            StartCoroutine(DelayExecution());
        }

        protected override void OnFall(float progress)
        {
            angle.x = Mathf.LerpAngle(angle.x, -90f, progress);

            tree.localEulerAngles = angle;

            root.rotation = rotation;
        }

        protected override void OnDie()
        {
            GameObject.Destroy(gameObject);
        }

        protected override IEnumerator DelayExecution()
        {
            yield return new WaitForSeconds(Time.deltaTime * time);

            if (TryGetComponent(out BoxCollider collider))
            {
                collider.isTrigger = true;
            }
        }

        protected enum HitType
        {
            Transform,
            Rigidbody,
            RigidbodyWithTransform,
            Special,
        }
    }
}