using UnityEngine;

namespace Game
{
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField] private Transform follower;

        [SerializeField] private Transform target;

        [SerializeField] private float radius = 5;

        [SerializeField] private Vector2 visibility = new Vector2(2.2f, 6);

        [SerializeField] private Vector2 field = new Vector2(-15, 90);

        [SerializeField, Range(0.1f, 10)] private float speed = 1f;

        private Quaternion _rotation, rotation;

        private Vector3 _position, position;

        private Vector3 offset;

        private Vector2 shift;

        private float distance;

        private RaycastHit hit;

        private void Awake()
        {
            distance = radius;

            shift = new Vector2(180, 0);
        }

        private void LateUpdate()
        {
            if (target == null) return;

            Handle(Time.deltaTime);

            _rotation = Quaternion.Euler(shift.y, shift.x + 180, 0);

            rotation = Quaternion.LerpUnclamped(rotation, _rotation, Time.deltaTime * speed);

            follower.rotation = rotation;

            _position = target.position + Offset(shift);

            position = Vector3.Lerp(position, _position, Time.deltaTime * speed);

            if (Physics.Linecast(position, target.position, out hit))
            {
                if (hit.transform.tag == "Wall")
                {
                    position = hit.point + hit.normal * 0.1f;
                }
            }
            follower.position = position;
        }

        private void Handle(float delta)
        {
#if UNITY_EDITOR
            shift.x += Input.GetAxis("Mouse X") * delta * 100f;
            shift.y -= Input.GetAxis("Mouse Y") * delta * 20f;

            distance -= Input.GetAxis("Mouse ScrollWheel") * delta * Mathf.Abs(distance) * 80;
#else
            //...
#endif
            shift.y = Mathf.Clamp(Utility.Math.AngleIn360(shift.y), field.x, field.y);

            distance = Mathf.Clamp(distance, visibility.x, visibility.y);
        }

        private Vector3 Offset(Vector2 angle)
        {
            offset.y = Mathf.Cos(angle.y * Mathf.Deg2Rad);

            offset.x = distance * Mathf.Sin(angle.x * Mathf.Deg2Rad) * offset.y;

            offset.z = distance * Mathf.Cos(angle.x * Mathf.Deg2Rad) * offset.y;

            offset.y = distance * Mathf.Sin(angle.y * Mathf.Deg2Rad);

            return offset;
        }
    }
}