using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.UI
{
    public class SnakeInformation
    {
        public Vector2 position;

        public float angle;

        public float length;

        public SnakeInformation(Vector2 position, float angle, float length)
        {
            this.length = length;

            this.angle = angle;

            this.position = position;
        }
    }
}