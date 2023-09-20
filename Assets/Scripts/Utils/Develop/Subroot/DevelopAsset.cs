using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Develop
{
    public class DevelopAsset : DevelopBase
    {
        public override void Initialize()
        {
            
        }

        public override void Refresh()
        {
            GUILayout.Label("1");

            GUILayout.Label("2");

            GUILayout.Label("3");

            GUILayout.Label("4");

            GUILayout.Button("5");
        }

        public override string Name => "Asset";
    }
}