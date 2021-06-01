using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataResource : ScriptableObject
    {
        public List<ResourceInformation> resources = new List<ResourceInformation>();

        public ResourceInformation Get(string key)
        {
            return null;
        }

        public bool Exist(string key)
        {
            return resources.Exists(x => x.key == key);
        }
    }
    [System.Serializable]
    public class ResourceInformation
    {
        public string key;

        public int capacity = -1;

        public Object prefab;

        public string secret;

        public string description;
    }
}