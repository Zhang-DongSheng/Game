using UnityEngine;

namespace Game.Model
{
    [DisallowMultipleComponent]
    public class ModelRotation : ItemBase
    {
        [SerializeField] private Transform target;

        [SerializeField] private Vector3 rotation;

        private Vector3 angle;

        protected override void OnUpdate(float delta)
        {
            if (target == null) return;

            angle = target.eulerAngles;

            if (angle.x != rotation.x || angle.y != rotation.y || angle.z != rotation.z)
            {
                target.eulerAngles = rotation;
            }
        }
    }
}