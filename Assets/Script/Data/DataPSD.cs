using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataPSD : ScriptableObject
    {
        public List<PSDInformation> list = new List<PSDInformation>();

        public void OnValidate()
        {
            
        }
    }
    [Serializable]
    public class PSDInformation
    {
        public string name;

        public List<SpriteInformation> sprites = new List<SpriteInformation>();

        public string description;
    }
    [Serializable]
    public class SpriteInformation
    {
        public string name;

        public Sprite sprite;

        public Vector2 position;

        public Vector2 size;

        public int order;

        public string description;
    }
}