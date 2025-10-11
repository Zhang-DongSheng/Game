using UnityEngine;

namespace Game.World
{
    public class Girl : Player
    {
        [SerializeField] GameObject[] weapons;

        public override void Born()
        {
            Trigger("");

            SwitchWeapon(1);
        }

        public override void Die()
        {
            Trigger("");

            SwitchWeapon(1);
        }

        public override void Walk()
        {
            Trigger("walk");

            SwitchWeapon(1);
        }

        public override void Run()
        {
            Trigger("run");

            SwitchWeapon(1);
        }

        public override void Idle()
        {
            Trigger("idel");

            SwitchWeapon(1);
        }

        public override void Jump()
        {
            Trigger("Jump");

            SwitchWeapon(1);
        }

        public override void Crouch()
        {

        }

        public override void Attack()
        {
            Trigger("attack1");

            SwitchWeapon(0);
        }

        public override void Damage()
        {
            Trigger("damage");

            SwitchWeapon(1);
        }

        public override void ReleaseSkill(int index)
        {
            switch(index)
            {
                case 0:
                    Trigger("attack1");
                    break;
                case 1:
                    Trigger("attack2");
                    break;
                default:
                    Trigger("attack3");
                    break;
            }
            SwitchWeapon(0);
        }

        private void SwitchWeapon(int index)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i] != null)
                {
                    weapons[i].SetActive(i == index);
                }
            }
        }
    }
}