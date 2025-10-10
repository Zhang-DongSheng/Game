using Game.State;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CameraController : MonoSingleton<CameraController>
    {
        [SerializeField] private new Camera camera;

        [SerializeField] private Transform target;

        [SerializeField] private float speed = 1;

        [SerializeField] private CameraParameters parameters;

        private Quaternion rotation;

        private Vector3 position;

        private float field;

        private void Awake()
        {
            position = camera.transform.position;

            rotation = camera.transform.rotation;

            field = camera.fieldOfView;

            parameters = new CameraParameters
            {
                field = field,
                position = position,
                rotation = rotation
            };
        }

        private void LateUpdate()
        {
            //current.OnUpdate(Time.deltaTime);

            position = Vector3.Lerp(position, parameters.position, Time.deltaTime * speed);

            rotation = Quaternion.Lerp(rotation, parameters.rotation, Time.deltaTime * speed);

            field = Mathf.Lerp(field, parameters.field, Time.deltaTime * speed);

            camera.transform.SetPositionAndRotation(position, rotation);

            camera.fieldOfView = field;
        }

        public void SwitchState<T>()
        {
            
        }

        public void SetTarget(Transform target)
        { 
            
        }

        public Transform Target => target;
        [System.Serializable]
        class CameraParameters
        {
            public float field;

            public Vector3 position;

            public Quaternion rotation;
        }
    }
}