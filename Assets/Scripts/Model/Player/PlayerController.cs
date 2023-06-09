using Game.Resource;
using UnityEngine;

namespace Game.Model
{
    public class PlayerController : Singleton<PlayerController>
    {
        private Player player;

        private Quaternion rotation;

        private Vector3 direction = new Vector3(0, 0, 0);

        public void SwitchPlayer(string name)
        {
            GameObject prefab = ResourceManager.Load<GameObject>(name);

            var clone = GameObject.Instantiate(prefab);

            player = clone.GetComponent<Player>();

            player.Born();
        }

        public void Move(Vector2 vector)
        {
            if (vector.x == 0 && vector.y == 0)
                return;
            direction.Set(vector.x, 0, vector.y);

            rotation = Quaternion.LookRotation(direction, Vector3.up);

            player.Move(direction);

            player.Raotate(rotation);

            if (vector.magnitude > 10)
            {
                player.Run();
            }
            else
            {
                player.Walk();
            }
        }

        public void Jump()
        {
            player.Jump();
        }

        public void Crouch()
        {
            player.Crouch();
        }

        public void Attack()
        {
            player.Attack();
        }

        public void ReleaseSkill(int index)
        {
            player.ReleaseSkill(index);
        }
    }
}