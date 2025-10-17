using Game.Data;
using Game.Network;
using Protobuf;
using System.Collections.Generic;
using UnityEngine;

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
                            uid = (uint)(i * 100 + j),
                            head = 1001 + (uint)Random.Range(0, 9),
                            frame = 2002,
                            relationship = i * -1 + 1,
                        });
                    }
                    _friends.Add(i, list);
                }
                ScheduleLogic.Instance.Update(Schedule.Friend);
            });
        }

        public void RequestFriendApply(uint uid, bool apply)
        {
            Friend friend = null;

            if (_friends.TryGetValue(1, out var list))
            {
                var index = list.FindIndex(x => x.uid == uid);

                if (index > -1)
                {
                    friend = list[index];

                    list.RemoveAt(index);
                }
            }
            // add
            if (apply && friend != null)
            {
                if (_friends.TryGetValue(0, out list))
                {
                    list.Add(friend);
                }
            }
            EventDispatcher.Post(UIEvent.Friend);
        }

        public void RequestFriendRemoveBlacklist(uint uid)
        {
            var key = 2;

            if (_friends.TryGetValue(key, out var list))
            {
                var index = list.FindIndex(x => x.uid == uid);

                if (index > -1)
                {
                    list.RemoveAt(index);
                }
            }
            EventDispatcher.Post(UIEvent.Friend);
        }
    }
}