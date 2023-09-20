using System.Collections.Generic;
using UnityEngine;

namespace Game.Develop
{
    public class DevelopView : MonoBehaviour
    {
        private string[] menu;

        private int index;

        private readonly List<DevelopBase> develops = new List<DevelopBase>();

        private void Awake()
        {
            develops.Add(new DevelopAsset());

            develops.Add(new DevelopLog());

            develops.Add(new DevelopScript());

            develops.Add(new DevelopSystem());

            foreach (var view in develops)
            {
                view.Register();
            }
            int count = develops.Count;

            menu = new string[count];

            for (int i = 0; i < count; i++)
            {
                menu[i] = develops[i].Name;
            }
        }

        private void OnDestroy()
        {
            foreach (var view in develops)
            {
                view.Unregister();
            }
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(Screen.width - 100, 0, 100, 100), "X"))
            {
                Close();
            }
            index = GUI.Toolbar(new Rect(0, 0, Screen.width - 100, 100), index, menu);

            if (index > -1)
            {
                GUILayout.BeginArea(new Rect(1, 100, Screen.width, Screen.height - 100));
                {
                    develops[index].Refresh();
                }
                GUILayout.EndArea();
            }
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}