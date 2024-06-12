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
    public class ClubLogic : Singleton<ClubLogic>, ILogic
    {
        public void Initialize()
        {

        }

        public void RequestClub()
        {
            var msg = new C2SClubRequest();

            NetworkManager.Instance.Send(NetworkMessageDefine.C2SClubRequest, msg, (handle) =>
            {
                ScheduleLogic.Instance.Update(Schedule.Club);
            });
        }
    }
}
