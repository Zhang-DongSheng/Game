using System.Collections.Generic;
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
    }
}