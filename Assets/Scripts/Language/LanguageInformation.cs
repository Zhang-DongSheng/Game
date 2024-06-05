using Data;
using System;
using System.Collections.Generic;

namespace Game
{
    [Serializable]
    public class LanguageInformation : InformationBase
    {
        public Language language;

        public string icon;

        public string font;

        public string url;

        public List<StringPair> words = new List<StringPair>();
    }
}