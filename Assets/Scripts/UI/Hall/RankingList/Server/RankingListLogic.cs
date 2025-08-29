using Game.Data;
using Game.Network;
using Protobuf;
using System.Collections.Generic;

namespace Game.Logic
{
    public class RankingListLogic : Singleton<RankingListLogic>, ILogic
    {
        private readonly Dictionary<int, List<Celebrity>> _rankings = new Dictionary<int, List<Celebrity>>();

        private readonly List<Celebrity> _empty = new List<Celebrity>(0);

        public void Initialize()
        {

        }

        public List<Celebrity> GetRankings(int index)
        {
            if (_rankings.TryGetValue(index, out var list))
            {
                return list;
            }
            return _empty;
        }

        public void RequestRankingList()
        {
            var msg = new C2SRankingListRequest();

            NetworkManager.Instance.Send(NetworkMessageDefine.C2SRankingListRequest, msg, (handle) =>
            {
                _rankings.Clear();

                for (int i = 0; i < 5; i++)
                {
                    var list = new List<Celebrity>();

                    int count = UnityEngine.Random.Range(3, 10);

                    for (int j = 0; j < count; j++)
                    {
                        list.Add(new Celebrity()
                        {
                            rank = j + 1,
                            name = "xxxx" + j,
                            head = 1,
                            frame = 1,
                        });
                    }
                    _rankings.Add(i, list);
                }
                ScheduleLogic.Instance.Update(Schedule.RankingList);
            });
        }
    }
}