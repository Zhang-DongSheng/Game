using Game.Model;
using Game.State;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class BattleView : ViewBase
    {
        [SerializeField] private UIJoyStick stick;

        [SerializeField] private Button jump;

        [SerializeField] private Button crouch;

        [SerializeField] private Button attack;

        [SerializeField] private List<Button> skills;

        protected override void OnAwake()
        {
            stick.onMove.AddListener(OnMove);

            jump.onClick.AddListener(OnClickJump);

            crouch.onClick.AddListener(OnClickCrouch);

            attack.onClick.AddListener(OnClickAttack);

            for (int i = 0; i < skills.Count; i++)
            {
                int index = i;

                if (skills[i] != null)
                {
                    skills[i].onClick.AddListener(() => OnClickSkill(index));
                }
            }
            PlayerController.Instance.SwitchPlayer("Package/Prefab/Model/Character/Female.prefab");
        }

        private void OnMove(Vector2 vector)
        {
            PlayerController.Instance.Move(vector);
        }

        private void OnClickJump()
        {
            PlayerController.Instance.Jump();
        }

        private void OnClickCrouch()
        {
            //PlayerController.Instance.Crouch();

            GameStateController.Instance.EnterState<GameSettlementState>();
        }

        private void OnClickAttack()
        {
            PlayerController.Instance.Attack();
        }

        private void OnClickSkill(int index)
        {
            PlayerController.Instance.ReleaseSkill(index);
        }
    }
}