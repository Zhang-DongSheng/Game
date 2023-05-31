using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class Dictionary
    {
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
    }
}