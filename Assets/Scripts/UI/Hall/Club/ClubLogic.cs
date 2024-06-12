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
        private Club _club;

        private readonly List<ClubMember> _members = new List<ClubMember>();

        public List<ClubMember> Members
        {
            get { return _members; }
        }

        public Club Club
        {
            get { return _club; }
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
