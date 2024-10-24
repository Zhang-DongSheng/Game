using UnityEngine;

namespace Game
{
    public class Follower : ItemBase
    {
        [SerializeField] private Transform target;

        [SerializeField] private Transform self;

        [SerializeField, Range(0, 360f)] private float angle = 90f;

        [SerializeField] private float offset = 1;

        private Vector3 position;

        protected override void OnUpdate(float delta)
        {
            position = Position(target.position, target.eulerAngles.y, offset);

            self.transform.position = position;
        }

        private Vector3 Position(Vector3 position, float angle, float offset)
        {
            angle -= this.angle;

            Vector3 vector = new Vector3()
            {
                x = Mathf.Sin(angle * Mathf.Deg2Rad) * offset,
                y = 0,
                z = Mathf.Cos(angle * Mathf.Deg2Rad) * offset,

            };
            return position + vector;
        }
    }
}