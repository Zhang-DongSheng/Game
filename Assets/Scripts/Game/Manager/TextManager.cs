using Data;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static Data.DataText;

namespace Game.UI
{
    public class TextManager : Singleton<TextManager>
    {
        private static DataText dictionary;

        public string Get(string key)
        {
            if (dictionary != null)
            {
                return dictionary.Get(key);
            }
            else
            {
                dictionary = DataManager.Instance.Load<DataText>();

                if (dictionary != null)
                {
                    return dictionary.Get(key);
                }
                else
                {
                    return key;
                }
            }
        }
    }
}
