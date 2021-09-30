using Data;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
    class DataBuilder
    {
        #region Create
        public static void Create_Resource()
        {
            Create<DataResource>("Resource");
        }
        [MenuItem("Data/Create/Language")]
        protected static void Create_DataLanguage()
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
        #endregion

        protected static void Create<T>(string file) where T : ScriptableObject
        {
            string path = string.Format("Assets/Package/Data/{0}.asset", file);

            if (AssetDatabase.LoadAssetAtPath(path, typeof(Object))) return;

            ScriptableObject script = ScriptableObject.CreateInstance<T>();
            path = string.Format("{0}/Package/Data", Application.dataPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = string.Format("Assets/Package/Data/{0}.asset", file);
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