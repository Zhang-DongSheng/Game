using System.Collections.Generic;

namespace Game.UI
{
    internal class NotificationLogic : Singleton<NotificationLogic>, ILogic
    {
        private readonly Dictionary<Notification, List<string>> _messages = new Dictionary<Notification, List<string>>();

        public void Initialize()
        {
        
        }

        public void Release()
        {
            
        }

        public void Push(Notification key, string value)
        {
            if (_messages.ContainsKey(key))
            {
                _messages[key].Add(value);
            }
            else
            {
                _messages.Add(key, new List<string>() { value });
            }
        }

        public string Pop(Notification key)
        {
            string value = string.Empty;

            if (_messages.TryGetValue(key, out List<string> list))
            {
                if (list.Count > 0)
                {
                    value = list[0]; list.RemoveAt(0);
                }
            }
            return value;
        }

        public bool Empty(Notification key)
        {
            if (_messages.ContainsKey(key))
            {
                return _messages[key].Count == 0;
            }
            return true;
        }
    }
}
