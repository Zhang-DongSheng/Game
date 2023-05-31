using Game;
using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataLanguage : DataBase
    {
        public List<Dictionary> dictionaries;

        public Dictionary Get(Language language)
        {
            int count = dictionaries.Count;

            for (int i = 0; i < count; i++)
            {
                if (dictionaries[i].language == language)
                {
                    return dictionaries[i];
                }
            }
            return count > 0 ? dictionaries[0] : null;
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
                    Dictionary dictionary = new Dictionary()
                    {
                        language = (Language)language,
                    };
                    int count = list.Count;

                    string key = language.ToString().ToLower();

                    for (int i = 0; i < count; i++)
                    {
                        dictionary.words.Add(new Word()
                        {
                            key = list[i].GetString("key"),
                            value = list[i].GetString(key)
                        });
                    }
                    dictionaries.Add(dictionary);
                }
            }
            else
            {
                Debuger.LogError(Author.Data, "多语言解析失败");
            }
        }

        public override void Clear()
        {
            dictionaries.Clear();
        }
    }
}