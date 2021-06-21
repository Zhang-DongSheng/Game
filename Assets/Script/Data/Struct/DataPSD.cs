using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataPSD : ScriptableObject
    {
        public List<PSDInformation> list = new List<PSDInformation>();

        public PSDInformation First
        {
            get 
            {
                if (list.Count > 0)
                {
                    return list[0];
                }
                return null;
            }
        }

        public PSDInformation Get(string index)
        {
            return list.Find(x => x.name == index);
        }
    }
    [Serializable]
    public class PSDInformation
    {
        public string name;

        public List<SpriteInformation> sprites = new List<SpriteInformation>();

        public Dictionary<string, List<SpriteInformation>> groups = new Dictionary<string, List<SpriteInformation>>();

        public string description;
    }
    [Serializable]
    public class SpriteInformation
    {
        public string name;

        public Texture2D texture;

        public Vector2 position;

        public Vector2 size;

        public int order;

        public string description;
    }
}