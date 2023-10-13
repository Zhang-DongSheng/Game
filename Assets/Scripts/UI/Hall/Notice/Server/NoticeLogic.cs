using System.Collections.Generic;

namespace Game.UI
{
    internal class NoticeLogic : Singleton<NoticeLogic>, ILogic
    {
        private readonly List<string> _messages = new List<string>();

        public void Initialize() { }

        public void Release() { }

        public void Push(string message)
        {
            _messages.Add(message);
        }

        public string Pop()
        {
            string value = string.Empty;

            if (_messages.Count > 0)
            {
                value = _messages[0]; _messages.RemoveAt(0);
            }
            return value;
        }

        public bool Empty
        {
            get
            {
                return _messages.Count == 0;
            }
        }
    }
}
