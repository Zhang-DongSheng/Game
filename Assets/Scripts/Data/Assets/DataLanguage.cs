using Game;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    public class DataLanguage : DataBase
    {
        public List<LanguageInformation> list = new List<LanguageInformation>();

        public override void Load(string content)
        {
            base.Load(content);

            foreach (var language in Enum.GetValues(typeof(Language)))
            {
                LanguageInformation information = new LanguageInformation()
                {
                    language = (Language)language,
                    icon = string.Format("language_{0}", language.ToString().ToLower()),
                    words = new List<StringPair>(),
                };
                int count = m_list.Count;

                string key = language.ToString().ToLower();

                for (int i = 0; i < count; i++)
                {
                    information.words.Add(new StringPair()
                    {
                        x = m_list[i].GetString("key"),
                        y = m_list[i].GetString(key)
                    });
                }
                list.Add(information);
            }
            Detection();
        }

        public override void Detection()
        {
            if (list.Count == 0) return;

            var words = list[0].words;

            var dic = new Dictionary<string, int>();

            foreach (var word in words)
            {
                if (dic.ContainsKey(word.x))
                {
                    Debuger.LogError(Author.Data, "language exist the same key:" + word.x);
                }
                else
                {
                    dic.Add(word.x, 1);
                }
            }
        }

        public override void Clear()
        {
            list = new List<LanguageInformation>();
        }
    }
    [Serializable]
    public class LanguageInformation : InformationBase
    {
        public Language language;

        public string icon;

        public string font;

        public string url;

        public List<StringPair> words = new List<StringPair>();
    }
}