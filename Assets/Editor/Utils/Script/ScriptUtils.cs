using Game;
using Game.Network;
using Game.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace UnityEditor
{
    public static class ScriptUtils
    {
        public static void CreateFromTemplate(string name, string path, string template = "001")
        {
            string source;

            switch (template)
            {
                case "001":
                    source = "Editor/Utils/Script/Template/001_CS_UIPanel.txt";
                    break;
                case "002":
                    source = "Editor/Utils/Script/Template/002_CS_UIPanel.txt";
                    break;
                case "003":
                    source = "Editor/Utils/Script/Template/003_CS_UIPanel.txt";
                    break;
                default:
                    source = "Editor/Utils/Script/Template/001_CS_UIPanel.txt";
                    break;
            }
            source = string.Format("{0}/{1}", Application.dataPath, source);

            string content = File.ReadAllText(source);

            content = content.Replace("#SCRIPTNAME#", name);

            CreateScript(path, content);
        }

        public static void CreateILRuntimeComponents(string path, HotfixComponents runtime)
        {
            var name = Path.GetFileNameWithoutExtension(path);

            int count = 0;

            if (runtime != null && runtime.components != null)
            {
                count = runtime.components.Count;
            }
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("// Don't modify, this is automatically generated");

            builder.AppendLine("using Game.UI;");

            builder.AppendLine("using UnityEngine;");

            builder.AppendLine("using UnityEngine.UI;");

            builder.AppendLine();

            builder.AppendLine("namespace ILRuntime.Game.UI");

            builder.AppendLine("{");

            builder.AppendLine($"\tpublic class {name}");

            builder.AppendLine("\t{");

            for (int i = 0; i < count; i++)
            {
                var content = runtime.components[i].ToDefineString();

                builder.AppendLine($"\t\t{content}");
            }
            if (count > 0) builder.AppendLine();

            builder.AppendLine("\t\tpublic void Relevance(Transform transform)");

            builder.AppendLine("\t\t{");

            for (int i = 0; i < count; i++)
            {
                var content = runtime.components[i].ToRelevanceString(runtime.transform);

                builder.AppendLine($"\t\t\t{content}");
            }
            if (count == 0) builder.AppendLine();

            builder.AppendLine("\t\t}");

            builder.AppendLine("\t}");

            builder.AppendLine("}");

            CreateScript(path, builder.ToString());
        }

        public static void CreateScript(string path, string content)
        {
            if (path.StartsWith("Assets/"))
            {
                path = Path.GetFullPath(path);
            }
            try
            {
                File.WriteAllText(path, content, new UTF8Encoding(false));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static void ModifyEnum(Type script, string parameter, out int index, bool addtive = true)
        {
            index = -1;

            if (!script.IsEnum) return;

            StringBuilder builder = new StringBuilder();

            string key; int value = 0;

            Dictionary<string, int> directory = new Dictionary<string, int>();

            if (addtive)
            {
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
            }

            if (directory.ContainsKey(parameter))
            {
                Debuger.LogError(Author.UI, "exist the same key!");
            }
            else
            {
                index = value + 1;

                directory.Add(parameter, index);
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

        public static void ModifyEnums(Type script, string[] parameters, bool addtive = true)
        {
            if (!script.IsEnum) return;

            if (parameters == null || parameters.Length == 0) return;

            StringBuilder builder = new StringBuilder();

            string key; int value = 0;

            Dictionary<string, int> directory = new Dictionary<string, int>();

            if (addtive)
            {
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

        public static void ModifyNetworkMessageDefine(Dictionary<string, int> parameters)
        {
            if (parameters == null || parameters.Count == 0) return;

            var script = typeof(NetworkMessageDefine);

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("// Don't modify, this is automatically generated");

            builder.AppendLine("using Protobuf;");

            builder.AppendLine("using Google.Protobuf;");

            builder.AppendLine();

            builder.AppendLine("namespace Game.Network");

            builder.AppendLine("{");

            builder.Append("\tpublic static class ");

            builder.AppendLine(script.Name);

            builder.AppendLine("\t{");

            foreach (var item in parameters)
            {
                builder.Append("\t\tpublic const int ");
                builder.Append(item.Key);
                builder.Append(" = ");
                builder.Append(item.Value);
                builder.AppendLine(";");
            }
            builder.AppendLine();

            builder.AppendLine("\t\tpublic static IMessage Deserialize(RawMessage raw)");

            builder.AppendLine("\t\t{");

            builder.AppendLine("\t\t\tswitch (raw.key)");

            builder.AppendLine("\t\t\t{");

            foreach (var item in parameters)
            {
                builder.Append("\t\t\t\tcase ");
                builder.Append(item.Key);
                builder.AppendLine(":");

                builder.Append("\t\t\t\t\treturn NetworkConvert.Deserialize<");
                builder.Append(item.Key);
                builder.AppendLine(">(raw.content);");
            }
            builder.AppendLine("\t\t\t}");

            builder.AppendLine("\t\t\treturn default;");

            builder.AppendLine("\t\t}");

            builder.AppendLine("\t}");

            builder.AppendLine("}");

            var path = Utility.Class.GetPath(script);

            if (string.IsNullOrEmpty(path)) return;

            File.WriteAllText(path, builder.ToString());

            AssetDatabase.Refresh();
        }
    }
}