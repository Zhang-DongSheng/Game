using Game;
using Game.Data;
using Game.UI;
using LitJson;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityEditor.Utils
{
    public static class AssetUtils
    {
        public static T Find<T>() where T : UnityEngine.Object
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);

            int count = guids.Length;

            if (count == 0)
            {
                Debuger.LogError(Author.Editor, $"Not Found {typeof(T).Name}!");
                return default;
            }
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);

            return AssetDatabase.LoadAssetAtPath<T>(path);
        }

        public static List<T> FindAll<T>() where T : UnityEngine.Object
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);

            int count = guids.Length;

            if (count == 0)
            {
                Debuger.LogError(Author.Editor, $"Not Found {typeof(T).Name}!");
                return default;
            }
            var assets = new List<T>(count);

            for (int i = 0; i < count; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);

                assets.Add(AssetDatabase.LoadAssetAtPath<T>(path));
            }
            return assets;
        }

        public static Dictionary<string, string> ScanKeysInPrefab()
        {
            var dic = new Dictionary<string, string>();

            var guids = AssetDatabase.FindAssets("t:Prefab");

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);

                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                var name = "[UI]" + prefab.name;

                var components = prefab.GetComponentsInChildren<TextBind>(true);

                foreach (var component in components)
                {
                    if (string.IsNullOrEmpty(component.content)) continue;

                    var key = component.content.ToLower();

                    if (dic.TryGetValue(key, out var value))
                    {
                        var list = value.Split(",");

                        if (list.Exist(name))
                        {
                            continue;
                        }
                        dic[key] += "," + name;
                    }
                    else
                    {
                        dic.Add(key, name);
                    }
                }
            }
            return dic;
        }

        public static Dictionary<string, string> ScanKeysInText()
        {
            var dic = new Dictionary<string, string>();

            return dic;

            var regex = new Regex(@"(?<=(""))[.\\s\\S]*?(?=(""))", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var guids = AssetDatabase.FindAssets("t:TextAsset");

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);

                var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

                var name = asset.name;

                var content = asset.text;

                var matches = regex.Matches(content);

                foreach (var match in matches)
                {
                    var key = match.ToString();

                    if (dic.TryGetValue(key, out _))
                    {
                        
                    }
                    else
                    {
                        dic.Add(key, "[Text]" + name);
                    }
                }
            }
            return dic;
        }

        public static Dictionary<string, string> ScanKeysInTable()
        {
            var dic = new Dictionary<string, string>();

            void Add(string key, string value)
            {
                if (dic.TryGetValue(key, out _))
                {
                    
                }
                else
                {
                    dic.Add(key, value);
                }
            }
            var types = Extension.GetChildrenTypes(typeof(DataBase), false);

            foreach (var type in types)
            {
                if (type == typeof(DataLanguage)) continue;

                var name = type.Name.ToLower()[4..];

                string path = $"{AssetPath.DataJson}/{name}.json";

                var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

                Debuger.Assert(asset != null, string.Format("{0} is null", path));

                if (asset == null) continue;

                string content = asset.text;

                var json = JsonMapper.ToObject(content);

                var list = json.GetJson("list");

                int count = list != null ? list.Count : 0;

                for (int i = 0; i < count; i++)
                {
                    var result = list[i].GetString("name");

                    if (!string.IsNullOrEmpty(result))
                    {
                        Add(result, "[Data]" + name);
                    }
                    result = list[i].GetString("descript");

                    if (!string.IsNullOrEmpty(result))
                    {
                        Add(result, "[Data]" + name);
                    }
                }
            }
            return dic;
        }
    }
}