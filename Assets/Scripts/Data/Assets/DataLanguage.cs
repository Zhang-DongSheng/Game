using LitJson;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Data
{
    public class DataLanguage : DataBase
    {
        public Language language;

        public string icon;

        public TMP_FontAsset font;

        public string url;

        public List<WordInformation> list;

        public override void Load(JsonData json)
        {
            Clear();

            int count = json.Count;

            string key = language.ToString().ToLower();

            for (int i = 0; i < count; i++)
            {
                list.Add(new WordInformation()
                {
                    key = json[i].GetString("key"),
                    value = json[i].GetString(key)
                });
            }
            Detection();
        }

        public override void Detection()
        {
            var keys = new Dictionary<string, int>();

            var values = new Dictionary<string, int>();

            foreach (var word in list)
            {
                if (keys.ContainsKey(word.key))
                {
                    Debug.LogError($"DataLanguage Detection Key Repeat : {word.key}");
                }
                else
                {
                    keys.Add(word.key, 1);
                }

                if (values.ContainsKey(word.value))
                {
                    Debug.LogError($"DataLanguage Detection Value Repeat : {word.value}");
                }
                else
                {
                    values.Add(word.value, 1);
                }
            }
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