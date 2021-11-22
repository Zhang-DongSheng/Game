using Data;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
    class DataBuilder
    {
        public const string PATH = "Assets/Package/Data";

        #region Create
        [MenuItem("Data/Create/Language")]
        protected static void Create_Language()
        {
            Create<DataLanguage>("Language");
        }
        [MenuItem("Data/Create/Currency")]
        protected static void Create_Currency()
        {
            Create<DataCurrency>("Currency");
        }
        [MenuItem("Data/Create/Prop")]
        protected static void Create_Prop()
        {
            Create<DataProp>("Prop");
        }
        [MenuItem("Data/Create/Task")]
        protected static void Create_Task()
        {
            Create<DataTask>("Task");
        }
        [MenuItem("Data/Create/Bone")]
        protected static void Create_Bone()
        {
            Create<DataBone>("Bone");
        }
        #endregion

        protected static void Create<T>(string file) where T : ScriptableObject
        {
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

        public static void Load<T>(string path, out T asset) where T : ScriptableObject
        {
            asset = AssetDatabase.LoadAssetAtPath<T>(path);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}