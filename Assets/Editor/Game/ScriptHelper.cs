using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityEditor.Game
{
    public static class ScriptHelper
    {
        public static Object Create(string path)
        {
            return CreateFromTemplate(path, null);
        }

        public static Object CreateFromTemplate(string path, string template)
        {
            string extension = Path.GetExtension(path);

            string name = Path.GetFileNameWithoutExtension(path);

            string full = Path.GetFullPath(path);

            if (File.Exists(full))
            {
                return AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
            }
            else
            {
                string source = string.Empty;

                switch (template)
                {
                    case null:
                        {
                            source = "Editor/Game/Template/001_CS_UIPanel.txt";
                        }
                        break;
                    default:
                        {
                            source = "Editor/Game/Template/001_CS_UIPanel.txt";
                        }
                        break;
                }

                try
                {
                    source = string.Format("{0}/{1}", Application.dataPath, source);

                    string content = File.ReadAllText(source);

                    content = content.Replace("#SCRIPTNAME#", name);

                    File.WriteAllText(full, content, new UTF8Encoding(false));

                    AssetDatabase.ImportAsset(path);

                    return AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
                }
                catch(Exception e)
                {
                    Debug.LogException(e);
                }
                return null;
            }
        }
    }
}
