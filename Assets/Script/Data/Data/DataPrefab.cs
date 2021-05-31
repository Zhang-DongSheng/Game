using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataPrefab : ScriptableObject
    {
        public List<PrefabInformation> resources = new List<PrefabInformation>();

        public PrefabInformation Get(string key)
        {
            return null;
        }

        public bool Exist(string key)
        {
            return resources.Exists(x => x.key == key);
        }
    }
    [System.Serializable]
    public class PrefabInformation
    {
        public string key;

        public int capacity = -1;

        public Object prefab;

        public string secret;

        public string description;
    }
}