using UnityEngine;

namespace Game.Model
{
    [DisallowMultipleComponent]
    public class ModelRotation : ItemBase
    {
        [SerializeField] private Transform target;

        [SerializeField] private float speed = 1;

        protected override void OnUpdate(float delta)
        {
            var x = Input.GetAxisRaw("Horizontal");

            if (x != 0)
            {
                Rotate(x);
            }
        }

        public void Rotate(float angle)
        {
            target.Rotate(Vector3.up, speed * angle);
        }
    }
}