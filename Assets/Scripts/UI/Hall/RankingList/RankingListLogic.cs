using Game.Network;
using Protobuf;
using System.Collections.Generic;

namespace Game.UI
{
    public class RankingListLogic : Singleton<RankingListLogic>, ILogic
    {
        private readonly Dictionary<int, List<RankingListPlayer>> rankinglist = new Dictionary<int, List<RankingListPlayer>>();

        private readonly List<RankingListPlayer> empty = new List<RankingListPlayer>(0);

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