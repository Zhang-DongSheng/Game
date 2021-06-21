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
        #endregion

        protected static void Create<T>(string name) where T : ScriptableObject
        {
            ScriptableObject script = ScriptableObject.CreateInstance<T>();
            string path = Application.dataPath + "/Resources/Data";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = string.Format("Assets/Resources/Data/{0}.asset", name);
            AssetDatabase.CreateAsset(script, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}