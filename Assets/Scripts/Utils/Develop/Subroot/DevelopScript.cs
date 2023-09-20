using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Develop
{
    public class DevelopScript : DevelopBase
    {
        private readonly string[] parameter = new string[2];

        public override void Initialize()
        {

        }

        public override void Refresh()
        {
            parameter[0] = GUI.TextField(new Rect(200, 100, 200, 100), parameter[0]);

            parameter[1] = GUI.TextField(new Rect(500, 100, 200, 100), parameter[1]);

            if (GUI.Button(new Rect(1500, 100, 200, 100), ""))
            {

            }
        }

        public override string Name => "Script";
    }
}