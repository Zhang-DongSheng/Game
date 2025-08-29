using Game.Data;
using System.Collections.Generic;

namespace Game.Logic
{
    internal class NotificationLogic : Singleton<NotificationLogic>, ILogic
    {
        private readonly Dictionary<NotificationType, List<Notification>> _notifies = new Dictionary<NotificationType, List<Notification>>();

        public void Initialize()
        {

        }

        public void Push(Notification notice)
        {
            if (_notifies.ContainsKey(notice.type))
            {
                _notifies[notice.type].Add(notice);
            }
            else
            {
                _notifies.Add(notice.type, new List<Notification>() { notice });
            }
        }

        public Notification Pop(NotificationType key)
        {
            if (_notifies.TryGetValue(key, out List<Notification> list))
            {
                if (list.Count > 0)
                {
                    var result = list[0];

                    list.RemoveAt(0);

                    return result;
                }
            }
            return null;
        }

        public bool Complete(NotificationType key)
        {
            if (_notifies.ContainsKey(key))
            {
                return _notifies[key].Count == 0;
            }
            return true;
        }
    }
}