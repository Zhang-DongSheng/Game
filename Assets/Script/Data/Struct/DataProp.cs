using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataProp : ScriptableObject
    {
        public List<PropInformation> props = new List<PropInformation>();
    }

    public class PropInformation
    {
        public int ID;

        public string name;

        public string icon;

        public float price;

        public string description;
    }
}