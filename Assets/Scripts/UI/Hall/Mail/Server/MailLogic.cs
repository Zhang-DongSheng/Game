using Data;
using System.Collections.Generic;

namespace Game
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

        public void Release()
        {
            
        }

        public void RequestMails()
        {
            int count = 10;

            for (int i = 0; i < count; i++)
            { 
                _mails.Add(new Mail()
                { 
                    content = i.ToString(),
                });
            }
            ScheduleLogic.Instance.Update(Schedule.Mail);
        }
    }
}