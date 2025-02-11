using Game.Data;
using Game.SM;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ItemDialogSystemPlayer : ItemBase
    {
        public int position;

        [SerializeField] private List<SMBase> _animations;

        [SerializeField] private ImageBind imgRole;

        private string role;

        public void Refresh(DialogRoleInformation info, string focus)
        {
            this.role = info.name;

            bool active = this.role == focus;

            imgRole.SetSprite(info.sprite);

            imgRole.SetColor(active ? Color.white : Color.gray);

            SetActive(true);
        }

        public void ShowOrHide(bool active, bool immediately = false)
        {
            int count = _animations.Count;

            for (int i = 0; i < count; i++)
            {
                _animations[i].Begin(active);
            }
        }
    }
}