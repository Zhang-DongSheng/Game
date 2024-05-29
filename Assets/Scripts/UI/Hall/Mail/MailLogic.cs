using Data;
using System.Collections.Generic;

namespace Game.UI
{
    public class MailLogic : Singleton<MailLogic>, ILogic
    {
        private readonly List<Mail> _mails = new List<Mail>();

        public void Initialize()
        {

        }

        public void RequestMails()
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
        }

        public List<Mail> Mails
        {
            get { return _mails; }
        }
    }
}