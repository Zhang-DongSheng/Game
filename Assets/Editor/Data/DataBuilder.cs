using Data;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
    class DataBuilder
    {
        #region Create
        [MenuItem("Data/Create/Config")]
        protected static void Create_DataConfig()
        {
            Create<ConfigInformation>("Config");
        }

        [MenuItem("Data/Create/Language")]
        protected static void Create_DataLanguage()
        {
            Create<DataLanguage>("Language");
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