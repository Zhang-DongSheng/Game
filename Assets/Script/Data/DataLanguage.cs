using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Data
{
    public class DataLanguage : ScriptableObject
    {
        [SerializeField] private List<Dictionary> m_data = new List<Dictionary>(Enum.GetValues(typeof(Language)).Length);

        [SerializeField] private Language m_language;

        private Dictionary dictionary;

#if UNITY_EDITOR
        [ContextMenu("Reload Language")]
        private void Reload()
        {
            string path = Application.streamingAssetsPath + "/language.txt";

            if (File.Exists(path))
            {
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    StreamReader reader = new StreamReader(stream);

                    JsonData list = JsonMapper.ToObject(reader.ReadToEnd());

                    reader.Dispose(); m_data.Clear();

                    if (list != null && list.IsArray)
                    {
                        foreach (var language in Enum.GetValues(typeof(Language)))
                        {
                            Dictionary dictionary = new Dictionary()
                            {
                                language = (Language)language,
                            };
                            for (int i = 0; i < list.Count; i++)
                            {
                                Word word = new Word()
                                {
                                    key = list[i].GetString(LanguageConfig.KEY),
                                };
                                switch ((Language)language)
                                {
                                    case Language.Chinese:
                                        word.value = list[i].GetString(LanguageConfig.Chinese);
                                        break;
                                    case Language.English:
                                        word.value = list[i].GetString(LanguageConfig.English);
                                        break;
                                }
                                dictionary.words.Add(word);
                            }
                            m_data.Add(dictionary);
                        }
                    }
                }
            }
        }
#endif

        #region Function
        public void Init()
        {
            for (int i = 0; i < m_data.Count; i++)
            {
                if (m_language == m_data[i].language)
                {
                    dictionary = m_data[i];
                    break;
                }
            }
        }

        public string Word(string key)
        {
            if (dictionary.TryParse(key, out string value))
            {
                return value;
            }
            return key;
        }

        public Font Font
        {
            get { return dictionary != null ? dictionary.font : null; }
        }
        #endregion
    }

    [System.Serializable]
    public class Dictionary
    {
        public string name;

        public Language language;

        public Font font;

        public string description;

        public List<Word> words = new List<Word>();

        public bool TryParse(string key, out string value)
        {
            int index = words.FindIndex(x => x.key == key);

            bool exist = index != -1 && index < words.Count;

            if (exist)
            {
                value = words[index].value;
            }
            else
            {
                value = string.Empty;
            }
            return exist;
        }
    }

    [System.Serializable]
    public class Word
    {
        public string key;

        public string value;
    }

    public enum Language
    {
        Chinese,
        English,
    }

    public static class LanguageConfig
    {
        public const string KEY = "key";

        public const string Chinese = "cn";

        public const string English = "en";
    }
}