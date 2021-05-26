using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class Config : ScriptableObject
    {
        public bool debug;

        public int[] version = new int[3];

        public string description;

        private void OnValidate()
        {
            if (version.Length != 3)
            {
                Debug.LogError("The Version Length is Error!");
            }
        }
    }
}