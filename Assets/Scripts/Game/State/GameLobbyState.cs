using Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.State
{
    public class GameLobbyState : IGameState
    {
        public void OnCreate()
        {

        }

        public void OnEnter()
        {
            UIQuickEntry.OpenSingle(UIPanel.UIMain);
        }

        public void OnExit()
        {

        }

        public void OnStay()
        {

        }
    }
}
