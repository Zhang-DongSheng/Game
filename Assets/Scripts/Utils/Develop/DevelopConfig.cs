using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Develop
{
    public static class DevelopConfig
    {
        public static GUIStyle title = new GUIStyle()
        {
            normal = new GUIStyleState()
            {
                textColor = Color.white,
            },
            fontSize = 40,
            alignment = TextAnchor.MiddleCenter,
        };

        public static GUIStyle normal = new GUIStyle()
        {
            normal = new GUIStyleState()
            {
                textColor = Color.white,
            },
            fontSize = 40,
            alignment = TextAnchor.MiddleCenter,
        };
    }
}