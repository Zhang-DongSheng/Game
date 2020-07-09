using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Data
{
    public class Data_Language : ScriptableObject
    {
        public List<Dictionary> m_data = new List<Dictionary>();

        public Language m_language;

        private Language _language;

        private readonly Dictionary<string, string> m_dictionary = new Dictionary<string, string>();

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_language != m_language)
            {
                _language = m_language;

                Switch();
            }
        }
#endif

        public void Switch(Language language)
        {
            m_language = language;

            Switch();
        }

        public void Switch()
        {
            m_dictionary.Clear();

            for (int i = 0; i < m_data.Count; i++)
            {
                if (m_data[i].language.Equals(m_language))
                {
                    for (int j = 0; j < m_data[i].words.Count; j++)
                    {
                        m_dictionary.Add(m_data[i].words[j].ID, m_data[i].words[j].content);
                    }
                    break;
                }
            }

            if (Application.isPlaying)
            {
                //Text_Helper[] helper = FindObjectsOfType<Text_Helper>();

                //for (int i = 0; i < helper.Length; i++)
                //{
                //    helper[i].SendMessage("Refresh");
                //}
            }
        }

        public string Word(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                if (m_dictionary.ContainsKey(key))
                {
                    return m_dictionary[key];
                }
            }
            return key;
        }
    }

    [System.Serializable]
    public class Dictionary
    {
        public string name;
        public Language language;
        public List<Word> words = new List<Word>();
    }

    [System.Serializable]
    public class Word
    {
        public string ID;
        public string content;
    }

    public enum Language
    {
        Chinese,
        English,
    }
}