using System;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class Bone : ISerializationCallbackReceiver
    {
        public string name;

        [NonSerialized] public Vector3 _position;

        [NonSerialized] public Quaternion _rotation;

        [NonSerialized] public Vector3 _scale;

        [SerializeField] private float[] position = new float[3];

        [SerializeField] private float[] rotation = new float[4];

        [SerializeField] private float[] scale = new float[3];

        Bone()
        {
            this._position = Vector3.zero;

            this._rotation = Quaternion.identity;

            this._scale = Vector3.one;
        }

        public void OnBeforeSerialize()
        {
            var serializedPosition = _position;

            var serializedRotation = _rotation;

            var serializedScale = _scale;

            Handle(ref serializedPosition, ref serializedRotation, ref serializedScale);

            position = new[] { serializedPosition.x, serializedPosition.y, serializedPosition.z };

            rotation = new[] { serializedRotation.x, serializedRotation.y, serializedRotation.z, serializedRotation.w };

            scale = new[] { serializedScale.x, serializedScale.y, serializedScale.z };
        }

        public void OnAfterDeserialize()
        {
            _position = new Vector3(position[0], position[1], position[2]);

            _rotation = new Quaternion(rotation[0], rotation[1], rotation[2], rotation[3]);

            _scale = new Vector3(scale[0], scale[1], scale[2]);

            Handle(ref _position, ref _rotation, ref _scale);
        }

        protected void Handle(ref Vector3 position, ref Quaternion rotation, ref Vector3 scale)
        {
            var spaceConvertMatrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(-1, 1, 1));

            var origMatrix = Matrix4x4.TRS(position, rotation, scale);

            var flippedMatrix = spaceConvertMatrix * origMatrix;

            position = new Vector3(flippedMatrix[0, 3], flippedMatrix[1, 3], flippedMatrix[2, 3]);

            rotation = QuaternionFromMatrix(flippedMatrix);
        }

        static Quaternion QuaternionFromMatrix(Matrix4x4 m)
        {
            return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
        }
    }
}