using Game;
using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataLanguage : DataBase
    {
        public List<LanguageInformation> languages;

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
            // 一定要记得去掉最后一行的逗号
            JsonData json = JsonMapper.ToObject(content);

            if (json.ContainsKey("list"))
            {
                JsonData list = json.GetJson("list");

                foreach (var language in Enum.GetValues(typeof(Language)))
                {
                    LanguageInformation information = new LanguageInformation()
                    {
                        language = (Language)language,
                        icon = string.Format("language_{0}", language.ToString().ToLower()),
                        dictionary = new Dictionary(),
                    };
                    int count = list.Count;

                    string key = language.ToString().ToLower();

                    for (int i = 0; i < count; i++)
                    {
                        information.dictionary.words.Add(new Word()
                        {
                            key = list[i].GetString("key"),
                            value = list[i].GetString(key)
                        });
                    }
                    languages.Add(information);
                }
            }
            else
            {
                Debuger.LogError(Author.Data, "多语言解析失败");
            }
        }

        public override void Clear()
        {
            languages.Clear();
        }
    }
}