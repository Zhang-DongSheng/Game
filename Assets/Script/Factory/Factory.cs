using System;
using System.Collections.Generic;

namespace UnityEngine.Factory
{
    public class Factory : MonoSingleton<Factory>
    {
        private readonly Dictionary<string, Workshop> shops = new Dictionary<string, Workshop>();

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            
        }

        public Object Pop(string key)
        {
            return null;
        }

        public void Push(string key, Object asset)
        {
            
        }
    }
}