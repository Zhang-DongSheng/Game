using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    public class DataLanguage : DataBase
    {
        public Language language;

        public string icon;

        public Font font;

        public string url;

        public List<WordInformation> list;

        public override void Load(string content)
        {
            base.Load(content);

            int count = m_list.Count;

            string key = language.ToString().ToLower();

            for (int i = 0; i < count; i++)
            {
                list.Add(new WordInformation()
                {
                    key = m_list[i].GetString("key"),
                    value = m_list[i].GetString(key)
                });
            }
            Detection();
        }

        public override void Detection()
        {

        }

        public override void Clear()
        {
            list = new List<WordInformation>();
        }
    }
    [Serializable]
    public class WordInformation : InformationBase
    {
        public string key;

        public string value;
    }
}