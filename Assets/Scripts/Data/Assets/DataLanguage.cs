using Game;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataLanguage : DataBase
    {
        public List<LanguageInformation> languages = new List<LanguageInformation>();

        public static LanguageInformation Get(Language language)
        {
            var data = DataManager.Instance.Load<DataLanguage>();

            if (data != null)
            {
                int count = data.languages.Count;

                for (int i = 0; i < count; i++)
                {
                    if (data.languages[i].language == language)
                    {
                        return data.languages[i];
                    }
                }
                return count > 0 ? data.languages[0] : null;
            }
            return null;
        }

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

                        value = m_list[i].GetString(key)
                    });
                }
                languages.Add(information);
            }
            Detection();
        }

        public override void Detection()
        {
            if (languages.Count == 0) return;

            var words = languages[0].dictionary.words;

            var dic = new Dictionary<string, int>();

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
                }
            }
        }

        public override void Clear()
        {
            languages = new List<LanguageInformation>();
        }
    }
}