using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Develop
{
    public class DevelopSystem : DevelopBase
    {
        public override void Initialize()
        {

        }

        public override void Refresh()
        {
            GUILayout.BeginVertical();
            {
                scroll = GUILayout.BeginScrollView(scroll);
                {
                    RefreshLine("name", SystemInfo.deviceName);
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        private void RefreshLine(string key, string value)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(key, GUILayout.Width(DevelopConfig.WIDTH_KEY));

                GUILayout.Label(value, GUILayout.ExpandWidth(true));
            }
            GUILayout.EndHorizontal();
        }

        public override string Name => "System";
    }
}