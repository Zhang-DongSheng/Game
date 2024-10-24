using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.UI
{
    public class SubDialogSystemPlayer : ItemBase
    {
        [SerializeField] private List<ItemDialogSystemPlayer> _players;

        public void Refresh()
        {
            int count = _players.Count;

            for (int i = 0; i < count; i++)
            {
                _players[i].Refresh(i.ToString());
            }
        }

        public void RefreshState(string role)
        {
            int count = _players.Count;

            for (int i = 0; i < count; i++)
            {
                _players[i].RefreshState(1);
            }
        }

        public void RefreshDisplay(bool active)
        { 
            int count = _players.Count;

            for (int i = 0; i < count; i++)
            {
                _players[i].OnClickShowOrHide(active);
            }
        }
    }
}
