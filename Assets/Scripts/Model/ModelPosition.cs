using UnityEngine;

namespace Game
{
    [ExecuteInEditMode]
    public class ModelPosition : MonoBehaviour
    {
        [SerializeField] private new Camera camera;

        [SerializeField] private Transform target;

        [SerializeField, Range(0, 1)] private float horizontal = 0.5f;

        [SerializeField, Range(0, 1)] private float vertical = 0.5f;

        [SerializeField] private Vector2 offset;

        private Vector3 position;

        private void Awake()
        {
            if (camera == null)
                camera = Camera.main;
            Execute();
        }

        private void OnValidate()
        {
            Execute();
        }

        public void Execute()
        {
            if (camera == null || target == null) return;

            float length = camera.transform.position.z - target.position.z;

            float x = horizontal - 0.5f;

            float y = vertical - 0.5f;

            Vector2 size = camera.Size(length);

            position.x = x * size.x + offset.x;

            position.y = y * size.y + offset.y;

            position.z = target.position.z;

            target.position = position;
        }
    }
}