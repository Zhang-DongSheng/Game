using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;

namespace Game.UI
{
    public class UIConsoleEntry : ItemBase
    {
        protected override void OnAwake()
        {
            GameObject.DontDestroyOnLoad(gameObject);
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(0, 0, 200, 50), "Console"))
            {
                UIQuickEntry.Open(UIPanel.UIConsole);
            }
        }
    }
}
