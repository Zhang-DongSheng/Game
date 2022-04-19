using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataText : DataBase
    {
        public Language language;

        public string icon;

        public Font font;

        public List<Word> words = new List<Word>();

        public string Word(string key)
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