using Game;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityEditor.Game
{
    public static class ScriptUtils
    {
        public static Object Create(string path)
        {
            return CreateFromTemplate(path, null);
        }

        public static Object CreateFromTemplate(string path, string template)
        {
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
                            source = "Editor/Extra/Script/Template/001_CS_UIPanel.txt";
                        }
                        break;
                    default:
                        {
                            source = "Editor/Extra/Script/Template/001_CS_UIPanel.txt";
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
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
                return null;
            }
        }

        public static void Modify(Type script, params string[] parameters)
        {
            StringBuilder builder = new StringBuilder();

            if (script.IsEnum)
            {
                var array = Enum.GetValues(script);

                string[] list = new string[array.Length];

                int index = 0;

                foreach (var v in array)
                {
                    list[index++] = v.ToString();
                }
                if (list.AllExist(parameters)) return;

                builder.AppendLine("// don't modify, this is auto editor!");

                builder.AppendLine("namespace Game.UI");

                builder.AppendLine("{");

                builder.Append("\tpublic enum ");

                builder.AppendLine(script.Name);

                builder.AppendLine("\t{");

                foreach (var item in list)
                {
                    builder.Append("\t\t");
                    builder.Append(item);
                    builder.AppendLine(",");
                }
                // ×·¼ÓÔÚºó±ß
                foreach (var item in parameters)
                {
                    if (list.Exist(item))
                    {
                        continue;
                    }
                    builder.Append("\t\t");
                    builder.Append(item);
                    builder.AppendLine(",");
                }
                builder.AppendLine("\t}");
                builder.AppendLine("}");
            }

            var path = Utility.Class.GetPath(script);

            if (string.IsNullOrEmpty(path)) return;

            File.WriteAllText(path, builder.ToString());

            AssetDatabase.Refresh();
        }
    }
}