using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class GluttonousSnakeView : ViewBase
    {
        private Vector2 rotation = new Vector2(1, 0);

        private Vector2 v = new Vector2(0, 0);

        [SerializeField] private float speedMin = 1;

        [SerializeField] private float speedMax = 10;

        [SerializeField] private ItemSnake snake;

        [SerializeField] private PrefabTemplate prefab;

        private readonly List<ItemSnake> snakes = new List<ItemSnake>();

        protected override void OnUpdate(float delta)
        {
            v.x = Input.GetAxis("Horizontal") * delta;

            v.y = Input.GetAxis("Vertical") * delta;

            if (v.x != 0 || v.y != 0)
            {
                rotation.x = v.x == 0 ? 0 : v.x > 0 ? 1 : -1;

                rotation.y = v.y == 0 ? 0 : v.y > 0 ? 1 : -1;

                snake.Turn(rotation);
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                snake.Growth();
            }

            if (Input.GetKey(KeyCode.Space))
            {
                snake.Accelerate(speedMax);
            }
            else
            {
                snake.Accelerate(speedMin);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                snake.Instantiate(Vector2.zero, rotation, speedMin);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                snake.Death();
            }
        }
    }
}