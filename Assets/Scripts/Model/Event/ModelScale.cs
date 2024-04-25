using UnityEngine;

namespace Game.Model
{
    [DisallowMultipleComponent]
    public class ModelScale : ItemBase
    {
        [SerializeField] private Transform target;

        private float scale = 1;

        protected override void OnUpdate(float delta)
        {
            var y = Input.GetAxisRaw("Vertical");

            if (y != 0)
            {
                Scale(y);
            }
        }

        public void Scale(float value)
        {
            scale += value * Time.deltaTime;

            scale = Mathf.Clamp(scale, 0, 10);

            target.localScale = Vector3.one * scale;
        }
    }
}