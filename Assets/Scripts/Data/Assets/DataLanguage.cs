using Game;
using System;
using System.Collections.Generic;

namespace Data
{
    public class DataLanguage : DataBase
    {
        public List<LanguageInformation> languages = new List<LanguageInformation>();

        public LanguageInformation Get(Language language)
        {
            int count = languages.Count;

            for (int i = 0; i < count; i++)
            {
                if (languages[i].language == language)
                {
                    return languages[i];
                }
            }
            return count > 0 ? languages[0] : null;
        }

        public override void Set(string content)
        {
            base.Set(content);

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
        }

        public override void Clear()
        {
            languages = new List<LanguageInformation>();
        }
    }
}