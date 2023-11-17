using System;
using System.Collections.Generic;

namespace Game
{
    [Serializable]
    public class LanguageInformation
    {
        public Language language;

        public string icon;

        public string font;

        public string url;

        public Dictionary dictionary;
    }
    [Serializable]
    public class Dictionary
    {
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
    }
    [Serializable]
    public class Word
    {
        public string key;

        public string value;
    }
}