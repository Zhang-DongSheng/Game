using Data;
using Game.UI;
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
        [MenuItem("Data/Create/Text")]
        protected static void Create_Text()
        {
            Create<DataText>();
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
            string file = typeof(T).Name;

            string path = string.Format("{0}/{1}.asset", PATH, file);

            if (AssetDatabase.LoadAssetAtPath(path, typeof(Object))) return;

            ScriptableObject script = ScriptableObject.CreateInstance<T>();
            path = string.Format("{0}/{1}", Application.dataPath, PATH.Remove(0, 7));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = string.Format("{0}/{1}.asset", PATH, file);
            AssetDatabase.CreateAsset(script, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}