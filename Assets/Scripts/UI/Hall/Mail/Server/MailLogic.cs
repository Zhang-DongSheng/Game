using Game.Data;
using Game.Network;
using Protobuf;
using System.Collections.Generic;

namespace Game.Logic
{
    public class MailLogic : Singleton<MailLogic>, ILogic
    {
        private readonly List<Mail> _mails = new List<Mail>();

        public List<Mail> Mails
        {
            get { return _mails; }
        }

        public void Initialize()
        {

        }

        public void RequestMails()
        {
            var msg = new C2SMailRequest()
            {
                
            };
            NetworkManager.Instance.Send(NetworkMessageDefine.C2SMailRequest, msg, (handle) =>
            {
                int count = 10;

                for (int i = 0; i < count; i++)
                {
                    _mails.Add(new Mail()
                    {
                        ID = (uint)i,
                        content = i.ToString(),
                    });
                }
                ScheduleLogic.Instance.Update(Schedule.Mail);
            });
        }
    }
}