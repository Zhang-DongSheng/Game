using Game.Network;
using Game.UI;
using Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class FriendLogic : Singleton<FriendLogic>, ILogic
    {
        public void Initialize()
        {

        }

        public void RequestFriend()
        {
            var msg = new C2SFriendRequest();

            NetworkManager.Instance.Send(NetworkMessageDefine.C2SFriendRequest, msg, (handle) =>
            {
                ScheduleLogic.Instance.Update(Schedule.Friend);
            });
        }
    }
}