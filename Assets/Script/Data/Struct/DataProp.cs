using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataProp : ScriptableObject
    {
        public List<PropInformation> props = new List<PropInformation>();

        public PropInformation Get(string key)
        {
            return props.Find(x => x.key == key);
        }
    }
    [Serializable]
    public class PropInformation
    {
        public string key;

        public string icon;

        public float price;

        public string description;
    }

    public class Prop
    {
        public int ID;

        public int number;

        public string key;
    }
}