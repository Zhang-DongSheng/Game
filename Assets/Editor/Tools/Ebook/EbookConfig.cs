using UnityEngine;

namespace UnityEditor.Ebook
{
    public class EbookConfig
    {
        private const string key_path = "ebook_path";

        private const string key_number = "ebook_number";

        public static string Path
        {
            get
            {
                if (PlayerPrefs.HasKey(key_path))
                {
                    return PlayerPrefs.GetString(key_path);
                }
                return string.Format("{0}/{1}", Application.dataPath.Substring(0, Application.dataPath.Length - 6), "Source/Ebook");
            }
            set
            {
                PlayerPrefs.SetString(key_path, value);
            }
        }

        public static int Number
        {
            get
            {
                if (PlayerPrefs.HasKey(key_number))
                {
                    return PlayerPrefs.GetInt(key_number);
                }
                return 1000;
            }
            set
            {
                PlayerPrefs.SetInt(key_number, value);
            }
        }
    }
}