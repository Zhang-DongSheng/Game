using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataLanguage : ScriptableObject
    {
        [SerializeField] private List<Dictionary> m_data = new List<Dictionary>();

        public Dictionary Dictionary(Language language)
        {
            return m_data.Find(x => x.language == language);
        }
        [ContextMenu("Initialize")]
        protected void Initialize()
        {
            int count = Enum.GetValues(typeof(Language)).Length;

            if (m_data.Count != count)
            {
                if (m_data.Count > count)
                {
                    while (m_data.Count > count)
                    {
                        m_data.RemoveAt(m_data.Count - 1);
                    }
                }
                else
                {
                    while (m_data.Count < count)
                    {
                        m_data.Add(new Dictionary());
                    }
                }

                for (int i = 0; i < count; i++)
                {
                    m_data[i].language = (Language)i;
                }
            }
        }
        [ContextMenu("Synchronization")]
        protected void Synchronization()
        {
            List<string> keys = new List<string>();

            for (int i = 0; i < m_data.Count; i++)
            {
                for (int j = 0; j < m_data[i].words.Count; j++)
                {
                    if (!keys.Contains(m_data[i].words[j].key))
                    {
                        keys.Add(m_data[i].words[j].key);
                    }
                }
            }

            for (int i = 0; i < m_data.Count; i++)
            {
                for (int j = 0; j < keys.Count; j++)
                {
                    if (m_data[i].words.Count > j)
                    {
                        if (m_data[i].words[j].key != keys[j])
                        {
                            m_data[i].words[j] = new Word()
                            {
                                key = keys[j],
                            };
                        }
                    }
                    else
                    {
                        m_data[i].words.Add(new Word()
                        {
                            key = keys[j],
                        });
                    }
                }
            }
        }
    }
    [System.Serializable]
    public class Dictionary
    {
        public Language language;

        public Sprite icon;

        public Font font;

        public List<Word> words = new List<Word>();
    }
    [System.Serializable]
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