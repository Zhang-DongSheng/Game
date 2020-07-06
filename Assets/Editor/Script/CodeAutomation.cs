using System.Data;
using System.IO;
using UnityEngine;

namespace UnityEditor.Script
{
    public class CodeAutomation
    {
        private const string SCRIPTPATH = "Script/";

        private const string SCRIPTEXTENSION = ".cs";

        [MenuItem("Assets/Create/C# Code/Data")]
        private static void CreateDataScript()
        {
            CreateScript(CodeType.Data, "Test");
        }

        [MenuItem("Assets/Create/C# Code/UI")]
        private static void CreateUIScript()
        {
            CreateScript(CodeType.UI, "Test");
        }

        public static void CreateDataScript(string name, DataTable table)
        {
            if (table != null && table.Rows.Count > 0)
            {
                PropertyParameter[] parameters = new PropertyParameter[table.Columns.Count];

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    parameters[i] = new PropertyParameter()
                    {
                        name = table.Rows[0][i].ToString(),
                        property = PropertyType.Property,
                        returned = ReturnType.String,
                    };
                }
                CreateScript(CodeType.Data, name, parameters);
            }
            else
            {
                CreateScript(CodeType.Data, name);
            }
        }

        private static void CreateScript(CodeType type, string name, params PropertyParameter[] parameters)
        {
            string[] path = GetScriptPath(type, name);

            FileStream stream = new FileStream(path[1], FileMode.OpenOrCreate);

            StreamWriter writer = new StreamWriter(stream);

            string content;

            #region Namespace
            writer.WriteLine("using System;");
            writer.WriteLine("using UnityEngine;");
            writer.WriteLine("using UnityEngine.UI;");
            writer.WriteLine("using System.Collections;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine();
            writer.WriteLine("namespace Script");
            #endregion

            writer.WriteLine("{");

            #region Class
            content = string.Format("\tpublic class {0}", path[0]);
            switch (type)
            {
                case CodeType.None:
                    content = string.Format("{0} : {1}", content, "MonoBehaviour");
                    break;
                case CodeType.Data:
                    content = string.Format("{0} : {1}", content, "ScriptableObject");
                    break;
                case CodeType.UI:
                    content = string.Format("{0} : {1}", content, "UIBase");
                    break;
                default:
                    content = string.Format("{0} : {1}", content, "MonoBehaviour");
                    break;
            }
            writer.WriteLine(content);
            writer.WriteLine("\t{");
            #endregion

            #region Attribute & Method

            switch (type)
            {
                case CodeType.Data:
                    writer.WriteLine(string.Format("\t\tpublic List<{0}Information> m_data = new List<{1}Information>();", path[0], path[0]));
                    writer.WriteLine();
                    writer.WriteLine(string.Format("\t\tpublic {0}Information Get(string key)", path[0]));
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\treturn null;");
                    writer.WriteLine("\t\t}");
                    break;
                default:
                    if (parameters.Length > 0)
                    {
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            Write(ref writer, parameters[i]);
                        }
                    }
                    else
                    {
                        writer.WriteLine("\t\t");
                    }
                    break;
            }
            #endregion

            writer.WriteLine("\t}");

            #region Extra Class
            switch (type)
            {
                case CodeType.Data:
                    writer.WriteLine();
                    writer.WriteLine("\t[System.Serializable]");
                    writer.WriteLine(string.Format("\tpublic class {0}Information", path[0]));
                    writer.WriteLine("\t{");
                    {
                        if (parameters.Length > 0)
                        {
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                Write(ref writer, parameters[i]);
                            }
                        }
                        else
                        {
                            writer.WriteLine("\t\t");
                        }
                    }
                    writer.WriteLine("\t}");
                    break;
            }
            #endregion

            writer.WriteLine("}");

            writer.Dispose();

            stream.Dispose();

            AssetDatabase.Refresh();
        }

        private static void Write(ref StreamWriter writer, PropertyParameter parameter)
        {
            string content;

            switch (parameter.property)
            {
                case PropertyType.Property:
                    content = string.Format("\t\tpublic {0} {1};", parameter.DataType, parameter.name);
                    writer.WriteLine(content);
                    break;
                case PropertyType.Method:
                    content = string.Format("\t\tpublic {0} {1}()", parameter.DataType, parameter.name);
                    writer.WriteLine(content);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\t");
                    writer.WriteLine("\t\t}");
                    break;
            }
        }

        private static string[] GetScriptPath(CodeType type, string name)
        {
            string folder = Path.Combine(Application.dataPath, SCRIPTPATH);

            switch (type)
            {
                case CodeType.None:
                    name = string.IsNullOrEmpty(name) ? "NewsScript" : name;
                    break;
                case CodeType.Data:
                    name = "Data" + name;
                    break;
                case CodeType.UI:
                    name = "UI" + name;
                    break;
                default:
                    name = string.IsNullOrEmpty(name) ? "NewsScript" : name;
                    break;
            }

            string path = string.Format("{0}{1}{2}", folder, name, SCRIPTEXTENSION);

            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            int index = 0;

            if (File.Exists(path))
            {
                while (File.Exists(path))
                {
                    path = string.Format("{0}{1}_{2}{3}", folder, name, ++index, SCRIPTEXTENSION);
                }
            }

            return new string[2] { index > 0 ? string.Format("{0}_{1}", name, index) : name, path };
        }
    }

    public class PropertyParameter
    {
        public string name;

        public PropertyType property;

        public ReturnType returned;

        public string DataType
        {
            get
            {
                switch (returned)
                {
                    case ReturnType.None:
                        return "void";
                    case ReturnType.String:
                        return "string";
                    case ReturnType.Int:
                        return "int";
                    case ReturnType.Float:
                        return "float";
                    default:
                        return "void";
                }
            }
        }
    }

    public enum PropertyType
    {
        Property,
        Method,
    }

    public enum ReturnType
    { 
        None,
        String,
        Int,
        Float,
        Double,
    }

    public enum CodeType
    {
        None,
        Data,
        UI,
        Count,
    }
}