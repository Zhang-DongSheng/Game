using System.Collections.Generic;

namespace Game
{
    public class RuntimeParameter
    {
        public readonly List<RuntimeEvent> events = new List<RuntimeEvent>();

        public void Register(RuntimeEvent key)
        {
            events.Add(key);
        }

        public void Unregister(RuntimeEvent key)
        {
            events.Remove(key);
        }

        public bool Exists(RuntimeEvent key)
        {
            return events.Contains(key);
        }
    }
}