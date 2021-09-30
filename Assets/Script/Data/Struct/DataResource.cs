using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataResource : DataBase
    {
        public List<ResourceInformation> resources = new List<ResourceInformation>();

        public bool Exist(string key)
        {
            return resources.Exists(x => x.key == key);
        }
    }
    [System.Serializable]
    public class ResourceInformation : InformationBase
    {
        public string key;

        public int capacity = -1;

        public ResourceType type;

        public Object asset;

        public string secret;

        public string description;
    }

    public enum ResourceType
    {
        GameObject,
        Asset,
        Atlas,
        Audio,
        Texture,
        Text,
        Movie,
    }
}