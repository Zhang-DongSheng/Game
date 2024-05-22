using Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityEditor
{
    public static class ScriptUtils
    {
        public static Object Create(string path)
        {
            string folder = Path.GetDirectoryName(path);

            if (Directory.Exists(folder) == false)
            {
                Directory.CreateDirectory(folder);
            }
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
                string source = "Editor/Utils/Script/Template/001_CS_UIPanel.txt";

                switch (template)
                {
                    case null:
                        {
                            source = "Editor/Utils/Script/Template/001_CS_UIPanel.txt";
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

        public static void ModifyEnum(Type script, params string[] parameters)
        {
            if (!script.IsEnum) return;

            if (parameters == null || parameters.Length == 0) return;

            StringBuilder builder = new StringBuilder();

            string key; int value = 0;

            Dictionary<string, int> directory = new Dictionary<string, int>();

            foreach (var e in Enum.GetValues(script))
            {
                key = e.ToString(); value = (int)e;

                if (directory.ContainsKey(key) || directory.ContainsValue(value))
                {
                    Debuger.LogError(Author.UI, "exist the same key!");
                }
                else
                {
                    directory.Add(key, value);
                }
            }
            int length = parameters.Length;

            for (int i = 0; i < length; i++)
            {
                if (directory.ContainsKey(parameters[i]))
                {
                    Debuger.LogError(Author.UI, "exist the same key!");
                }
                else
                {
                    directory.Add(parameters[i], ++value);
                }
            }
            builder.AppendLine("// Don't modify, this is automatically generated");

            builder.AppendLine("namespace Game.UI");

            builder.AppendLine("{");

            builder.Append("\tpublic enum ");

            builder.AppendLine(script.Name);

            builder.AppendLine("\t{");

            foreach (var item in directory)
            {
                builder.Append("\t\t");
                builder.Append(item.Key);
                builder.Append(" = ");
                builder.Append(item.Value);
                builder.AppendLine(",");
            }
            builder.AppendLine("\t}");

            builder.AppendLine("}");

            var path = Utility.Class.GetPath(script);

            if (string.IsNullOrEmpty(path)) return;

            File.WriteAllText(path, builder.ToString());

            AssetDatabase.Refresh();
        }
    }
}