using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class Talent
    {
        public int ID;

        public string name;

        public bool active;

        public int relevance;

        public Vector2 position;

        public Vector3Int neighbours;
    }
}
