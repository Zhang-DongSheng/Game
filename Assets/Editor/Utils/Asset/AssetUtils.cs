using Game;
using Game.UI;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
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
                Debug.LogError($"Not Found {typeof(T).Name}!");
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
                Debug.LogError($"Not Found {typeof(T).Name}!");
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
            var regex = new Regex(@"(?<=(""))[.\\s\\S]*?(?=(""))", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var dic = new Dictionary<string, string>();

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

            return dic;
        }
    }
}