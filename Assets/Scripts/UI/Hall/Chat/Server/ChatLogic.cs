using Game;
using Game.Network;
using Game.UI;
using Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.UI
{
    public class ChatLogic : Singleton<ChatLogic>, ILogic
    {
        public void Initialize()
        {

        }

        public void RequestChat()
        {
            ScheduleLogic.Instance.Update(Schedule.Chat);
        }
    }
}