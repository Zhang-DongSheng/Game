using UnityEngine;

namespace Game
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private new Transform camera;

        [SerializeField] private Transform target;

        [SerializeField] private Vector3 offset;

        [SerializeField, Range(0.1f, 10)] private float speed = 1f;

        [SerializeField, Range(0, 90)] private float angle = 45f;

        private Quaternion _rotation, rotation;

        private Vector3 _position, position;

        private Vector3 _offset;

        private Vector3 eulerAngles;

        private void Start()
        {
            if (target != null)
            {
                _position = target.position;
                _rotation = target.rotation;
            }
        }

        private void LateUpdate()
        {
            position = target.position + Offset(target.localEulerAngles.y);

            _position = Vector3.Lerp(_position, position, Time.deltaTime * speed);

            rotation = target.rotation;

            eulerAngles = rotation.eulerAngles;

            eulerAngles.x = this.angle;

            rotation.eulerAngles = eulerAngles;

            _rotation = Quaternion.LerpUnclamped(_rotation, rotation, Time.deltaTime * speed);

            camera.position = _position;

            camera.rotation = _rotation;
        }

        private Vector3 Offset(float angle)
        {
            _offset.x = offset.z * Mathf.Sin(angle * Mathf.Deg2Rad);

            _offset.y = offset.y;

            _offset.z = offset.z * Mathf.Cos(angle * Mathf.Deg2Rad);

            return _offset;
        }
    }
}