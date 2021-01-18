using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Data
{
    public class DataBuilder : MonoBehaviour
    {
        #region Create
        [MenuItem("Data/Create/Language")]
        private static void Create_DataLanguage()
        {
            Create<DataLanguage>("Language");
        }

        [MenuItem("Data/Create/Config")]
        private static void Create_DataConfig()
        {
            Create<DataConfig>("Config");
        }
        #endregion

        #region Load
        [MenuItem("Data/Load/Language")]
        private static void Load_DataLanguage()
        {
            DataLanguage data = Resources.Load<DataLanguage>("Data/Language");

            if (data != null)
            {
                
            }
            else
            {
                EditorUtility.DisplayDialog("Warning！", "请先创建数据！", "关闭");
            }

            AssetDatabase.SaveAssets();

            AssetDatabase.Refresh();
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