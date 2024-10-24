using Game.SM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.UI
{
    public class ItemDialogSystemPlayer : ItemBase
    {
        [SerializeField] private List<SMBase> _animations;

        [SerializeField] private ImageBind imgRole;

        public void Refresh(string role)
        {

        }

        public void RefreshState(int state)
        {
            
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
