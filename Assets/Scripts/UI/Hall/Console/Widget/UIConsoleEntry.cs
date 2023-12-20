using UnityEngine;

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
            if (GUI.Button(new Rect(500, 0, 200, 100), "Console"))
            {
                UIQuickEntry.Open(UIPanel.UIConsole);
            }
        }
    }
}
