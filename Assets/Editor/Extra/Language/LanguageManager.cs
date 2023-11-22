using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    public class LanguageManager
    {
        private static readonly LanguageManager _instance = new LanguageManager();
        public static LanguageManager Instance { get { return _instance; } }
        private LanguageManager()
        {
            worlds = new Dictionary<string, string>();

            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Editor/Extra/Language/language.txt");

            if (asset == null || string.IsNullOrEmpty(asset.text)) return;

            string content = asset.text.Replace("\r\n", string.Empty);

            string[] parameters = content.Split(',');

            string[] value;

            int count = parameters.Length;

            for (int i = 0; i < count; i++)
            {
                value = parameters[i].Split('-');

                if (value.Length < 2) continue;

                if (worlds.ContainsKey(value[0].ToLower()))
                {
                    continue;
                }
                worlds.Add(value[0].ToLower(), value[1]);
            }
            Debuger.LogWarning(Author.Editor, "×Öµä´Ê¿âÊýÁ¿" + worlds.Count);
        }

        private readonly Dictionary<string, string> worlds;

        public string GetWorld(string key)
        {
            if (worlds.TryGetValue(key.ToLower(), out string value))
            {
                return value;
            }
            return key;
        }
    }
}