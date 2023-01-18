using System;
using System.Collections.Generic;

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
        public Language language;

        public string icon;

        public string font;

        public Dictionary dictionary;

        public string Get(string key)
        {
            for (int i = 0; i < dictionary.words.Count; i++)
            {
                if (dictionary.words[i].key == key)
                {
                    return dictionary.words[i].value;
                }
            }
            return key;
        }

        protected override void Editor()
        {
            
        }
        [Serializable]
        public class Dictionary
        {
            public List<Word> words = new List<Word>();
        }
        [Serializable]
        public class Word
        {
            public string key;

            public string value;
        }
    }
}