using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    //[CreateAssetMenu(fileName = "Lanuage", menuName = "New Lanuage")]
    public class LanuageData : ScriptableObject
    {
        public List<LanuageInformation> list;
    }
    [System.Serializable]
    public class LanuageInformation
    {
        public SystemLanguage language;

        public List<WordInformation> words;
    }
    [System.Serializable]
    public class WordInformation
    {
        public string key;

        public string value;
    }
}