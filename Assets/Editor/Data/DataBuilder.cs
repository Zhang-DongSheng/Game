using Data;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
    class DataBuilder
    {
        public const string PATH = "Assets/Package/Data";

        #region Create
        [MenuItem("Data/Create/Config")]
        protected static void Create_Config()
        {
            Create<DataConfig>();
        }
        [MenuItem("Data/Create/UI")]
        protected static void Create_UI()
        {
            Create<DataUI>();
        }
        [MenuItem("Data/Create/Language")]
        protected static void Create_Text()
        {
            Create<DataLanguage>();
        }
        [MenuItem("Data/Create/Sprite")]
        protected static void Create_Sprite()
        {
            Create<DataSprite>();
        }
        [MenuItem("Data/Create/Prop")]
        protected static void Create_Prop()
        {
            Create<DataProp>();
        }
        #endregion

        public static void Create<T>() where T : ScriptableObject
        {
            Create<T>(string.Format("{0}/{1}.asset", PATH, typeof(T).Name));
        }

        public static void Create<T>(string path) where T : ScriptableObject
        {
            if (AssetDatabase.LoadAssetAtPath(path, typeof(Object))) return;

            ScriptableObject script = ScriptableObject.CreateInstance<T>();
            string folder = string.Format("{0}/{1}", Application.dataPath, Path.GetDirectoryName(path).Remove(0, 7));
            if (Directory.Exists(folder) == false)
                Directory.CreateDirectory(folder);
            AssetDatabase.CreateAsset(script, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}