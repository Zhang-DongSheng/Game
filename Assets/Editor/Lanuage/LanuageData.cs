using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    /// <summary>
    /// ±‡º≠∆˜∂‡”Ô—‘
    /// </summary>
    /// [CreateAssetMenu(fileName = "Lanuage", menuName = "New Lanuage")]
    public class LanuageData : ScriptableObject
    {
        public List<LanuageInformation> list;
        [ContextMenu("Reference")]
        protected void Reference()
        {
            LanuageManager.Initialize();
        }
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