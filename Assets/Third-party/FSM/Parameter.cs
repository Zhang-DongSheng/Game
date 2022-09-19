using System.Collections.Generic;

namespace FSM
{
    public class Parameter
    {
        private readonly Dictionary<string, int> parameters = new Dictionary<string, int>();

        public int GetInt(string key)
        {
            if (parameters.ContainsKey(key))
            {
                return parameters[key];
            }
            return -1;
        }

        public bool GetBool(string key)
        {
            if (parameters.ContainsKey(key))
            {
                return parameters[key].Equals(1);
            }
            return false;
        }

        public void SetInt(string key, int value)
        {
            if (parameters.ContainsKey(key))
            {
                parameters[key] = value;
            }
            else
            {
                parameters.Add(key, value);
            }
        }

        public void SetBool(string key, bool value)
        {
            if (parameters.ContainsKey(key))
            {
                parameters[key] = value ? 1 : 0;
            }
            else
            {
                parameters.Add(key, value ? 1 : 0);
            }
        }
    }
}