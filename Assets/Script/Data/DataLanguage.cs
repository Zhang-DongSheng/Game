using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Data
{
    public class DataLanguage : ScriptableObject
    {
        public List<Dictionary> m_data = new List<Dictionary>();

        public void Load()
        {
            string path_folder = Application.streamingAssetsPath + "/Language";

            if (Directory.Exists(path_folder))
            {
                DirectoryInfo dicInfo = new DirectoryInfo(path_folder);
                FileInfo[] _files = dicInfo.GetFiles();
                List<FileInfo> files = _files.ToList();

                m_data.Clear();

                foreach (FileInfo file in files)
                {
                    string[] file_name = file.Name.Split('.');

                    if (file_name.Length == 2 && file_name[1] == "txt")
                    {
                        string[] param = file_name[0].Split('_');

                        if (int.TryParse(param[1], out int value))
                        {
                            if (value >= 0 && value < Enum.GetValues(typeof(Language)).Length)
                            {
                                Dictionary dic = new Dictionary
                                {
                                    language = (Language)value,
                                };
                                dic.name = dic.language.ToString();

                                using (FileStream fs = new FileStream(file.FullName, FileMode.OpenOrCreate))
                                {
                                    StreamReader sr = new StreamReader(fs);

                                    string content = sr.ReadLine();

                                    while (!string.IsNullOrEmpty(content))
                                    {
                                        string[] _temp = content.Split(' ');

                                        if (_temp.Length == 2 && !string.IsNullOrEmpty(_temp[0]))
                                        {
                                            Word word = new Word()
                                            {
                                                key = _temp[0],
                                                value = _temp[1]
                                            };
                                            dic.words.Add(word);
                                        }

                                        content = sr.ReadLine();
                                    }
                                }
                                m_data.Add(dic);
                            }
                        }
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class Dictionary
    {
        public string name;

        public Language language;

        public string description;

        public List<Word> words = new List<Word>();
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
}