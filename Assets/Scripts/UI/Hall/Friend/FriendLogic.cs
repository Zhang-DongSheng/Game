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
        private readonly Dictionary<int, List<Friend>> _friends = new Dictionary<int, List<Friend>>();

        private readonly List<Friend> _empty = new List<Friend>(0);

        public List<Friend> Friends
        {
            get
            {
                if (_friends.TryGetValue(0, out var list))
                { 
                    return list;
                }
                return _empty;
            }
        }

        public List<Friend> Apply
        {
            get
            {
                if (_friends.TryGetValue(1, out var list))
                {
                    return list;
                }
                return _empty;
            }
        }

        public List<Friend> Blacklist
        {
            get
            {
                if (_friends.TryGetValue(2, out var list))
                {
                    return list;
                }
                return _empty;
            }
        }


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