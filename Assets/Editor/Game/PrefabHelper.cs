using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace UnityEditor.Game
{
    public static class PrefabHelper
    {
        public static GameObject Create(string path)
        {
            string extension = Path.GetExtension(path);

            string name = Path.GetFileNameWithoutExtension(path);

            GameObject entity = new GameObject(name);

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(entity, path, out bool success);

            GameObject.DestroyImmediate(entity);

            PrefabUtility.SavePrefabAsset(prefab);

            return prefab;
        }

        public static void Modify(string path, Action<GameObject> callback)
        {
            GameObject prefab = PrefabUtility.LoadPrefabContents(path);

            callback?.Invoke(prefab);

            PrefabUtility.SavePrefabAsset(prefab);
        }
    }
}