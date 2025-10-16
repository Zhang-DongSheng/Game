using Game.Data;
using Game.Network;
using Protobuf;
using System.Collections.Generic;

namespace Game.Logic
{
    public class FriendLogic : Singleton<FriendLogic>, ILogic
    {
        private readonly Dictionary<int, List<Friend>> _friends = new Dictionary<int, List<Friend>>();

        private readonly List<Friend> _empty = new List<Friend>(0);

        public void Initialize()
        {

        }

        public List<Friend> GetFriends(int index)
        {
            if (_friends.TryGetValue(index, out var list))
            {
                return list;
            }
            return _empty;
        }

        public void RequestFriend()
        {
            var msg = new C2SFriendRequest();

            NetworkManager.Instance.Send(NetworkMessageDefine.C2SFriendRequest, msg, (handle) =>
            {
                _friends.Clear();

                for (int i = 0; i < 3; i++)
                {
                    var list = new List<Friend>();

                    int count = UnityEngine.Random.Range(3, 10);

                    for (int j = 0; j < count; j++)
                    {
                        list.Add(new Friend()
                        {
                            name = "xxxx" + j,
                            head = 1001,
                            frame = 2002,
                            relationship = i * -1 + 1,
                        });
                    }
                    _friends.Add(i, list);
                }
                ScheduleLogic.Instance.Update(Schedule.Friend);
            });
        }
    }
}