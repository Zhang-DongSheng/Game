using System;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class SerializableBone : ISerializationCallbackReceiver
    {
        public string name;

        [NonSerialized] public Vector3 position;

        [NonSerialized] public Quaternion rotation;

        [NonSerialized] public Vector3 scale;

        [SerializeField] private float[] _position = new float[3];

        [SerializeField] private float[] _rotation = new float[4];

        [SerializeField] private float[] _scale = new float[3];

        public bool Serializable { get; set; }

        SerializableBone()
        {
            this.position = Vector3.zero;

            this.rotation = Quaternion.identity;

            this.scale = Vector3.one;
        }

        public void OnBeforeSerialize()
        {
            if (Serializable)
            {
                _position = new[] { position.x, position.y, position.z };

                _rotation = new[] { rotation.x, rotation.y, rotation.z, rotation.w };

                _scale = new[] { scale.x, scale.y, scale.z };
            }
        }

        public void OnAfterDeserialize()
        {
            position = new Vector3(_position[0], _position[1], _position[2]);

            rotation = new Quaternion(_rotation[0], _rotation[1], _rotation[2], _rotation[3]);

            scale = new Vector3(_scale[0], _scale[1], _scale[2]);
        }
    }
}