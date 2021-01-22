using UnityEngine;

namespace Game
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private new Transform camera;

        [SerializeField] private Transform target;

        [SerializeField] private Vector3 offset;

        [SerializeField, Range(0.1f, 50)] private float speed;

        private Vector3 current, position;

        private Vector3 velocity = Vector3.zero;

        private void Awake()
        {
            if (camera == null)
                camera = transform;
        }

        public void Target(Transform target)
        {
            this.target = target;
        }

        private void LateUpdate()
        {
            if (camera == null || target == null) return;

            position = target.localPosition + offset;

            if (current.Distance(position) < 0.1f) return;

            current = Vector3.SmoothDamp(current, position, ref velocity, Time.deltaTime * speed);

            camera.localPosition = current;
        }
    }
}