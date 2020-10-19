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
        private static void Create_Data_Language()
        {
            Create_Data<DataLanguage>("Language");
        }
        #endregion

        #region Load
        [MenuItem("Data/Load/Language")]
        private static void Load_Data_Language()
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

        private static void Create_Data<T>(string name) where T : ScriptableObject
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