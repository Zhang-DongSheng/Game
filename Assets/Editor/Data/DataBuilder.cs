using System.IO;
using UnityEditor;
using UnityEngine;

namespace Data
{
    public class DataBuilder : MonoBehaviour
    {
        #region Create
        [MenuItem("Data/Create/Config")]
        private static void Create_DataConfig()
        {
            Create<DataConfig>("Config");
        }

        [MenuItem("Data/Create/Language")]
        private static void Create_DataLanguage()
        {
            Create<DataLanguage>("Language");
        }
        #endregion

        private static void Create<T>(string name) where T : ScriptableObject
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