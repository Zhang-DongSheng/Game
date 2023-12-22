using UnityEngine;

namespace Game.Model
{
    public class ModelDisplay : ItemBase
    {
        private const float MIN = 30, MAX = 100;

        [SerializeField] private GameObject self;

        [SerializeField] private Transform eye;

        [SerializeField] private Transform pedestal;

        [SerializeField, Range(0.1f, 3)] private float speed = 1;

        [SerializeField, Range(MIN, MAX)] private float zoom;

        protected override void OnAwake()
        {

        }

        protected override void OnUpdate(float delta)
        {
            var x = Input.GetAxisRaw("Horizontal");

            var y = Input.GetAxisRaw("Vertical");

            if (x != 0 || y != 0)
            {
                Rotate(x);
            }
        }

        public void Rotate(float angle)
        {
            eye.Rotate(Vector3.up * speed * angle);
        }

        public void Zoom(float value)
        {
            zoom += value;

            zoom = Mathf.Clamp(zoom, MIN, MAX);
        }

        public void SetPedestal(GameObject target)
        {
            target.transform.SetParent(pedestal);
        }

        public void Show()
        {
            SetActive(self, true);
        }

        public void Hide()
        {
            SetActive(self, false);
        }
    }
}