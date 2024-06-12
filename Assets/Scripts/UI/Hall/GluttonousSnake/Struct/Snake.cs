using UnityEngine;

namespace Game.UI
{
    public class Snake
    {
        public Vector2 position;

        public float angle;

        public float length;

        public Snake(Vector2 position, float angle, float length)
        {
            this.length = length;

            this.angle = angle;

            this.position = position;
        }
    }
}