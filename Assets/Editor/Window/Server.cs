using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Window;
using UnityEngine;

namespace UnityEditor.Window
{
    public class Server : CustomWindow
    {
        private const string PATHEXE = "../GameServer/bin/Debug/GameServer.exe";

        private const string PATHPROJECT = "../GameServer/GameServer.sln";

        [MenuItem("Game/Server")]
        protected static void Open()
        {
            Open<Server>("·þÎñÆ÷");
        }

        protected override void Initialise()
        {
            Detection();
        }

        protected override void Refresh()
        {
            if (GUILayout.Button(ToLanguage("Startup")))
            {
                string path = PATHEXE;

                EditorUtility.OpenWithDefaultApp(path);
            }

            if (GUILayout.Button(ToLanguage("Open Project")))
            {
                string path = PATHPROJECT;

                EditorUtility.OpenWithDefaultApp(path);
            }

            if (GUILayout.Button(ToLanguage("Fetch Project")))
            {
                Application.OpenURL("https://github.com/Zhang-DongSheng/GameServer");
            }
        }

        private void Detection()
        { 
            
        }
    }
}
