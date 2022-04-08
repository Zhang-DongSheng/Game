using System;
using Object = UnityEngine.Object;

namespace Game.Resource
{
    public class ResRequest
    {
        public string key;

        public string url;

        public string path;

        public Action<Object> success;

        public Action fail;
    }
}