using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ItemSnake : ItemBase
    {
        [SerializeField] private RectTransform target;

        [SerializeField] private PrefabTemplate prefab;

        [SerializeField, Range(1, 100)] private float size = 50;

        [SerializeField, Range(0, 10)] private float step = 1;

        private Vector2 direction = new Vector2(1, 0);

        private bool alive = false;

        private float length, magnitude;

        private float speed = 1;

        private Vector2 position;

        private Vector3 rotation;

        private Vector2 vector;

        private readonly int[] count = new int[] { 0, 0 };

        private readonly List<ItemSnakeBody> bodies = new List<ItemSnakeBody>();

        private readonly List<SnakeInformation> snakes = new List<SnakeInformation>();

        protected override void OnUpdate(float delta)
        {
            if (!alive) return;

            vector = direction * delta * speed;

            magnitude += vector.magnitude;

            position += vector;

            target.anchoredPosition = position;

            rotation.z = Vector2.SignedAngle(Vector2.right, direction);

            target.rotation = Quaternion.Euler(rotation);

            if (magnitude > step)
            {
                snakes.Add(new SnakeInformation(position, rotation.z, magnitude));

                magnitude = 0;
            }
            count[0] = snakes.Count;

            count[1] = bodies.Count; length = 0;

            float final = (count[1] + 1) * size;

            for (int i = count[0] - 1; i > -1; i--)
            {
                length += snakes[i].length;

                if (length > final)
                {
                    snakes.RemoveAt(i);
                }
            }
            // Body
            for (int i = 0; i < count[1]; i++)
            {
                TryGetSnakeInformation(i, out Vector2 position, out float angle);

                bodies[i].Move(position, angle);
            }
        }

        public void Instantiate(Vector2 position, Vector2 direction, float speed)
        {
            this.position = position;

            this.direction = direction;

            this.speed = speed;

            this.alive = true;
        }

        public void Turn(Vector2 direction)
        {
            this.direction = direction;
        }

        public void Accelerate(float speed)
        {
            this.speed = speed;
        }

        public void Growth()
        {
            int count = bodies.Count;

            TryGetSnakeInformation(count, out Vector2 position, out float angle);

            var body = prefab.Create<ItemSnakeBody>();

            body.Instantiate(position, angle);

            bodies.Add(body);
        }

        public void Death()
        {
            this.alive = false;

            int count = bodies.Count;

            for (int i = 0; i < count; i++)
            {
                GameObject.Destroy(bodies[i].gameObject);
            }
            bodies.Clear();
        }

        public void TryGetSnakeInformation(int index, out Vector2 position, out float angle)
        {
            int count = snakes.Count;

            if (count > 0)
            {
                position = snakes[0].position;

                angle = snakes[0].angle;

                float current = (index + 1) * size;

                float length = 0;

                for (int i = count - 1; i > -1; i--)
                {
                    length += snakes[i].length;

                    if (length >= current)
                    {
                        if (i > 0)
                        {
                            float progress = 1 - (length - current) / snakes[i].length;

                            position = Vector2.Lerp(snakes[i].position, snakes[i - 1].position, progress);

                            angle = Mathf.Lerp(snakes[i].angle, snakes[i - 1].angle, progress);
                        }
                        else
                        {
                            position = snakes[i].position;

                            angle = snakes[i].angle;
                        }
                        break;
                    }
                }
            }
            else
            {
                position = this.position;

                angle = Vector2.Angle(Vector2.up, direction);
            }
        }
    }
}