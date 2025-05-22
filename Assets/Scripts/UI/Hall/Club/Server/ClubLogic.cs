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
        private readonly List<Club> _clubs = new List<Club>();

        private readonly List<Member> _members = new List<Member>();

        public List<Member> Members
        {
            get { return _members; }
        }

        public Club Club
        {
            get { return _clubs[0]; }
        }

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
