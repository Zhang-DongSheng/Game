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
    public class RankingListLogic : Singleton<RankingListLogic>, ILogic
    {
        public void Initialize()
        { 
        
        }

        public void RequestRankingList()
        {
            var msg = new C2SRankingListRequest();

            NetworkManager.Instance.Send(NetworkMessageDefine.C2SRankingListRequest, msg, (handle) =>
            {
                ScheduleLogic.Instance.Update(Schedule.RankingList);
            });
        }
    }
}