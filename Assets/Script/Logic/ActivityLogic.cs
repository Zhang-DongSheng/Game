using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ActivityLogic : Singleton<ActivityLogic>
    {
        private readonly List<string> opened = new List<string>();

        public void Popup()
        {
            if (Popup("1"))
            {

            }
            else if (Popup("2"))
            {

            }
            else if (Popup("3"))
            {

            }
        }

        private bool Popup(string key)
        {
            if (opened.Contains(key))
            {
                return false;
            }
            else
            {
                opened.Add(key);

                switch (key)
                {
                    case "":

                        break;
                    default:

                        break;
                }
                return true;
            }
        }
    }
}
