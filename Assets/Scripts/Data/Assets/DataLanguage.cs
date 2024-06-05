using Game;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
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
                    dictionary = new Dictionary(),
                };
                int count = m_list.Count;

                string key = language.ToString().ToLower();

                for (int i = 0; i < count; i++)
                {
                    information.dictionary.words.Add(new Word()
                    {
                        key = m_list[i].GetString("key"),

                        value = m_list[i].GetString(key),

                        hashcode = m_list[i].GetString("key").GetHashCode(),
                    });
                }
                list.Add(information);
            }
            Detection();

            Sort();
        }

        public override void Detection()
        {
            if (list.Count == 0) return;

            var words = list[0].dictionary.words;

            var dic = new Dictionary<string, int>();

            var hash = new List<int>();

            int count = words.Count;

            for (int i = 0; i < count; i++)
            {
                if (dic.ContainsKey(words[i].key))
                {
                    Debuger.LogError(Author.Data, "language exist the same key:" + words[i].key);
                }
                else
                {
                    dic.Add(words[i].key, 1);

                    if (hash.Contains(words[i].hashcode))
                    {
                        Debuger.LogError(Author.Data, "language exist the same hashcode:" + words[i].key);
                    }
                    hash.Add(words[i].hashcode);
                }
            }
        }

        public override void Clear()
        {
            list = new List<LanguageInformation>();
        }
    }
}