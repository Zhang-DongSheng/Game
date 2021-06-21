using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataLanguage : ScriptableObject
    {
        public List<Dictionary> dictionaries = new List<Dictionary>();

        public Dictionary Dictionary(Language language)
        {
            return dictionaries.Find(x => x.language == language);
        }
        [ContextMenu("Initialize")]
        protected void Initialize()
        {
            int count = Enum.GetValues(typeof(Language)).Length;

            if (dictionaries.Count != count)
            {
                if (dictionaries.Count > count)
                {
                    while (dictionaries.Count > count)
                    {
                        dictionaries.RemoveAt(dictionaries.Count - 1);
                    }
                }
                else
                {
                    while (dictionaries.Count < count)
                    {
                        dictionaries.Add(new Dictionary());
                    }
                }

                for (int i = 0; i < count; i++)
                {
                    dictionaries[i].language = (Language)i;
                }
            }
        }
        [ContextMenu("Synchronization")]
        protected void Synchronization()
        {
            List<string> keys = new List<string>();

            for (int i = 0; i < dictionaries.Count; i++)
            {
                for (int j = 0; j < dictionaries[i].words.Count; j++)
                {
                    if (!keys.Contains(dictionaries[i].words[j].key))
                    {
                        keys.Add(dictionaries[i].words[j].key);
                    }
                }
            }

            for (int i = 0; i < dictionaries.Count; i++)
            {
                for (int j = 0; j < keys.Count; j++)
                {
                    if (dictionaries[i].words.Count > j)
                    {
                        if (dictionaries[i].words[j].key != keys[j])
                        {
                            dictionaries[i].words[j] = new Word()
                            {
                                key = keys[j],
                            };
                        }
                    }
                    else
                    {
                        dictionaries[i].words.Add(new Word()
                        {
                            key = keys[j],
                        });
                    }
                }
            }
        }
    }
    [Serializable]
    public class Dictionary
    {
        public Language language;

        public Sprite icon;

        public Font font;

        public List<Word> words = new List<Word>();
    }
    [Serializable]
    public class Word
    {
        public string key;

        public string value;
    }
    /// <summary>
    /// 语言
    /// </summary>
    public enum Language
    {
        Chinese,
        English,
    }
}