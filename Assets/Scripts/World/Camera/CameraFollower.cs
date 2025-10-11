using UnityEngine;

namespace Game
{
    public class CameraFollower : ItemBase
    {
        [SerializeField] private Transform follower;

        [SerializeField] private Transform target;

        [SerializeField] private LayerMask layer;

        [SerializeField] private float radius = 5;

        [SerializeField] private Vector2 visibility = new Vector2(2.2f, 6);

        [SerializeField] private Vector2 field = new Vector2(-15, 90);

        [SerializeField, Range(0.1f, 10)] private float speed = 1f;

        private Vector3 position;

        private Vector3 offset;

        private Vector3 shift, spring;

        private float distance;

        private RaycastHit hit;

        protected override void OnAwake()
        {
            distance = radius;

            shift = new Vector3(180, 0, 0);
        }

        protected override void OnUpdate(float delta)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                spring = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                shift += (Input.mousePosition - spring) * delta;

                spring = Input.mousePosition;
            }
            distance -= Input.GetAxis("Mouse ScrollWheel") * delta * Mathf.Abs(distance) * 80;
#else
            //...
#endif
            shift.y = Mathf.Clamp(shift.y, field.x, field.y);

            distance = Mathf.Clamp(distance, visibility.x, visibility.y);
        }

        protected override void OnLateUpdate(float delta)
        {
            if (target == null) return;

            position = target.position + Offset(shift, distance);

            position = Vector3.Lerp(follower.position, position, delta * speed);

            if (Vector3.Distance(target.position, position) < visibility.x)
            {
                position = target.position + (position - target.position).normalized * visibility.x;
            }
            else if (Physics.Linecast(position, target.position, out hit, layer))
            {
                position = hit.point + hit.normal * 0.1f;
            }
            follower.position = position;

            follower.LookAt(target, Vector3.up);
        }

        private Vector3 Offset(Vector2 angle, float distance)
        {
            offset.y = Mathf.Cos(angle.y * Mathf.Deg2Rad);

            offset.x = distance * Mathf.Sin(angle.x * Mathf.Deg2Rad) * offset.y;

            offset.z = distance * Mathf.Cos(angle.x * Mathf.Deg2Rad) * offset.y;

            offset.y = distance * Mathf.Sin(angle.y * Mathf.Deg2Rad);

            return offset;
        }
    }
}