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
            List<string> roles = new List<string>()
            { 
                "ÂÀÐã²Å",
                "¼§ÎÞÃü"
            };
            int count = _players.Count;

            for (int i = 0; i < count; i++)
            {
                if (roles.Count > i)
                {
                    _players[i].Refresh(roles[i]);
                }
                else
                {
                    _players[i].SetActive(false);
                }
            }
        }

        public void RefreshState(string role)
        {
            int count = _players.Count;

            for (int i = 0; i < count; i++)
            {
                _players[i].RefreshState(role);
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
