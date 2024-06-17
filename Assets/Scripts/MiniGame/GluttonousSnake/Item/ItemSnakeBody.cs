using UnityEngine;

namespace Game.UI
{
    public class ItemSnakeBody : ItemBase
    {
        [SerializeField] private RectTransform target;

        private Vector3 rotation = Vector3.zero;

        public void Instantiate(Vector2 position, float angle)
        {
            Move(position, angle);
        }

        public void Move(Vector2 position, float angle)
        {
            target.anchoredPosition = position;

            rotation.z = angle;

            target.rotation = Quaternion.Euler(rotation);
        }
    }
}