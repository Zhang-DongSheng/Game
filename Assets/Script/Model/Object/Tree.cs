using UnityEngine;

namespace Game.Model.Object
{
    [RequireComponent(typeof(BoxCollider))]
    public class Tree : MonoBehaviour
    {
        [SerializeField] private Transform root;

        [SerializeField] private HitType type;

        [SerializeField, Range(0.1f, 10f)] private float speed = 1f;

        [SerializeField] private bool trigger;

        private Vector3 vector;

        private Vector3 angle;

        private float step;

        private Status status;

        private void Awake()
        {
            if (TryGetComponent(out BoxCollider collider))
            {
                collider.isTrigger = trigger;
            }
            status = Status.Alive;
        }

        private void Update()
        {
            switch (status)
            {
                case Status.Hit:
                    {
                        root.localEulerAngles = angle;

                        transform.localEulerAngles = -angle;

                        status = Status.Fall;
                    }
                    break;
                case Status.Fall:
                    {
                        step += Time.deltaTime * speed;

                        angle.x = Mathf.Lerp(0, -90, step);

                        root.localEulerAngles = angle;

                        if (step > 1)
                        {
                            status = Status.Die;
                        }
                    }
                    break;
                case Status.Die:
                    {
                        status = Status.Death;
                    }
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (status != Status.Alive) return;

            status = Status.Hit;

            switch (type)
            {
                case HitType.Transform:
                    {
                        vector = other.transform.position - transform.position;
                    }
                    break;
                case HitType.Rigidbody:
                    {
                        Rigidbody rigidbody = other.attachedRigidbody;

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
                        Rigidbody rigidbody = other.attachedRigidbody;

                        if (rigidbody != null)
                        {
                            Vector3 v1 = rigidbody.velocity;

                            Vector3 v2 = other.transform.position - transform.position;

                            vector = (v1.normalized + v2.normalized) * 0.5f;
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

            angle.y = Vector3.Angle(vector.Vector3To2(), Vector2.up);

            status = Status.Hit;
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnTriggerEnter(collision.collider);
        }

        enum HitType
        {
            Transform,
            Rigidbody,
            RigidbodyWithTransform,
            Special,
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