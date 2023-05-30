using Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.State
{
    /// <summary>
    /// 登录账号
    /// </summary>
    public class GameLoginState : IGameState
    {
        public void OnCreate()
        {

        }

        public void OnEnter()
        {
            UIQuickEntry.OpenSingle(UIPanel.UILogin);
        }

        public void OnExit()
        {

        }

        public void OnStay()
        {

        }
    }
}
