using Game.Attribute;
using UnityEngine;

namespace Game.World
{
    public class CameraController : MonoSingleton<CameraController>
    {
        [FieldName("主摄像机")]
        [SerializeField] private Camera main;
        [FieldName("速度")]
        [SerializeField] private float speed = 1;
        [FieldName("参数")]
        [SerializeField] private CameraParameters parameters;

        private Quaternion rotation;

        private Vector3 position;

        private float field;

        private void Awake()
        {
            position = main.transform.position;

            rotation = main.transform.rotation;

            field = main.fieldOfView;

            parameters = new CameraParameters
            {
                field = field,
                position = position,
                rotation = rotation
            };
        }

        private void LateUpdate()
        {
            position = Vector3.Lerp(position, parameters.position, Time.deltaTime * speed);

            rotation = Quaternion.Lerp(rotation, parameters.rotation, Time.deltaTime * speed);

            field = Mathf.Lerp(field, parameters.field, Time.deltaTime * speed);

            main.transform.SetPositionAndRotation(position, rotation);

            main.fieldOfView = field;
        }

        public void SetPosition(Vector3 position)
        {
            parameters.position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            parameters.rotation = rotation;
        }

        public void SetField(float field)
        {
            parameters.field = field;
        }
        [System.Serializable]
        class CameraParameters
        {
            public float field;

            public Vector3 position;

            public Quaternion rotation;
        }
    }
}