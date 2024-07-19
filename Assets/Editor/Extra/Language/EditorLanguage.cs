using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    public static class EditorLanguage
    {
        private static readonly Dictionary<string, string> worlds = new Dictionary<string, string>();
        [InitializeOnLoadMethod]
        public static void Load()
        {
            worlds.Clear();

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
        }

        public static string GetWorld(string key)
        {
            string result = key;

            if (worlds.TryGetValue(key.ToLower(), out result))
            {
                return result;
            }
            else
            {
                var strings = key.ToLower().Split(' ');

                int length = strings.Length;

                if (length > 0)
                {
                    result = string.Empty;

                    for (int i = 0; i < length; i++)
                    {
                        if (!worlds.TryGetValue(strings[i], out string value))
                        {
                            value = strings[i];
                        }
                        // First char don't add space 
                        if (i == 0)
                            result = value;
                        else
                            result = string.Format("{0} {1}", result, value);
                    }
                }
                return result;
            }
        }
    }
}