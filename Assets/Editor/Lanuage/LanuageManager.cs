using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    public static class LanuageManager
    {
        private static LanuageData m_data;

        private static readonly Dictionary<string, string> words = new Dictionary<string, string>();
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            m_data = AssetDatabase.LoadAssetAtPath<LanuageData>("Assets/Editor/Lanuage/Lanuage.asset");

            Debug.LogError("多语言加载成功");
        }

        public static string Get(string key)
        {
            if (words.ContainsKey(key))
            {
                return words[key];
            }
            else
            {
                words.Add(key, key);
            }
            // Loading...
            if (m_data != null)
            {
                var current = m_data.list.Find(x => x.language == Application.systemLanguage);

                if (current != null)
                {
                    var word = current.words.Find(x => x.key == key);

                    if (word != null)
                    {
                        words[key] = word.value;
                    }
                }
            }
            return words[key];
        }
    }
}