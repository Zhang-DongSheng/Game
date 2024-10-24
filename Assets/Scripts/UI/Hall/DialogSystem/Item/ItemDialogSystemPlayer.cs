using Game.SM;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ItemDialogSystemPlayer : ItemBase
    {
        [SerializeField] private List<SMBase> _animations;

        [SerializeField] private ImageBind imgRole;

        private string role;

        public void Refresh(string role)
        {
            this.role = role;

            SetActive(true);
        }

        public void RefreshState(string role)
        {
            bool active = this.role == role;

            imgRole.SetColor(active ? Color.white : Color.gray);
        }

        public void OnClickShowOrHide(bool active)
        {
            int count = _animations.Count;

            for (int i = 0; i < count; i++)
            {
                _animations[i].Begin(active);
            }
        }
    }
}