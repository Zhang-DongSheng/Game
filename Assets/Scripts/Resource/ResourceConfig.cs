using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Resource
{
    public static class ResourceConfig
    {
        public static void Initialize()
        {

        }

        public static string Path(LoadType type, string name)
        {
            switch (type)
            {
                case LoadType.Resources:
                    return Utility.Path.GetPathWithoutExtension(name);
                case LoadType.AssetBundle:
                    return string.Format("{0}/{1}", Application.persistentDataPath, name);
                case LoadType.AssetDatabase:
                    return string.Format("Assets/{0}/{1}", "Package", name);
                default:
                    return name;
            }
        }
    }
}