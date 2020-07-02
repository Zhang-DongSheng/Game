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
        [MenuItem("数据/创建/Language")]
        private static void Create_Data_Language()
        {
            Create_Data<Data_Language>("Language");
        }
        #endregion

        #region Load
        [MenuItem("数据/加载/Language")]
        private static void Load_Data_Language()
        {
            Data_Language data = Resources.Load<Data_Language>("Data/Language");

            if (data != null)
            {
                string path_folder = Application.streamingAssetsPath + "/Language";

                if (Directory.Exists(path_folder))
                {
                    DirectoryInfo dicInfo = new DirectoryInfo(path_folder);
                    FileInfo[] _files = dicInfo.GetFiles();
                    List<FileInfo> files = _files.ToList();

                    data.m_data.Clear();

                    foreach (FileInfo file in files)
                    {
                        string[] file_name = file.Name.Split('.');

                        if (file_name.Length == 2 && file_name[1] == "txt")
                        {
                            string[] param = file_name[0].Split('_');

                            if (int.TryParse(param[1], out int value))
                            {
                                if (value >= 0 && value < Enum.GetValues(typeof(Language)).Length)
                                {
                                    Dictionary dic = new Dictionary
                                    {
                                        language = (Language)value,
                                    };
                                    dic.name = dic.language.ToString();

                                    using (FileStream fs = new FileStream(file.FullName, FileMode.OpenOrCreate))
                                    {
                                        StreamReader sr = new StreamReader(fs);

                                        string content = sr.ReadLine();

                                        while (!string.IsNullOrEmpty(content))
                                        {
                                            string[] _temp = content.Split(' ');

                                            if (_temp.Length == 2 && !string.IsNullOrEmpty(_temp[0]))
                                            {
                                                Word word = new Word()
                                                {
                                                    ID = _temp[0],
                                                    content = _temp[1]
                                                };
                                                dic.words.Add(word);
                                            }

                                            content = sr.ReadLine();
                                        }
                                    }
                                    data.m_data.Add(dic);
                                }
                            }
                        }
                    }
                }
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