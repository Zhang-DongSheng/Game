using Game;
using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataText : DataBase
    {
        public enum Language
        {
            Chinese,
            English,
            Japanese,
        }
        [Serializable]
        public class Word
        {
            public string key;

            public string value;
        }
        public Language language;

        public string icon;

        public string font;

        public List<Word> words = new List<Word>();

        public string Get(string key)
        {
            for (int i = 0; i < words.Count; i++)
            {
                if (words[i].key == key)
                {
                    return words[i].value;
                }
            }
            return key;
        }

        public override void Set(string content)
        {
            base.Set(content);
            // 一定要记得去掉最后一行的逗号
            JsonData json = JsonMapper.ToObject(content);

            if (json.ContainsKey("list"))
            {
                JsonData list = json.GetJson("list");

                string language = this.language.ToString().ToLower();

                int count = list.Count;

                for (int i = 0; i < count; i++)
                {
                    words.Add(new Word()
                    {
                        key = list[i].GetString("key"),
                        value = list[i].GetString(language)
                    });
                }
            }
            else
            {
                Debuger.LogError(Author.Data, "多语言解析失败");
            }
        }

        public override void Clear()
        {
            words = new List<Word>();
        }
    }
}