using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor.Window
{
    class AutomationCode : CustomWindow
    {
        private string code;

        private int index;

        private readonly List<PropertyParameter> parameters = new List<PropertyParameter>();

        protected override string Title { get { return "代码生成"; } }
        [MenuItem("Script/Editor")]
        protected static void Open()
        {
            Open<AutomationCode>();
        }

        protected override void Init() { }

        protected override void Refresh()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(GUILayout.Width(50));
                {
                    foreach (var value in Enum.GetValues(typeof(PropertyType)))
                    {
                        PropertyType property = (PropertyType)value;

                        GUI.color = GetColor(property);

                        if (GUILayout.Button(property.ToString()))
                        {
                            parameters.Add(new PropertyParameter()
                            {
                                property = property,
                                order = parameters.Count,
                                old = parameters.Count,
                                name = string.Format("{0}{1}", property, index),
                                ID = index++
                            });
                        }
                    }
                }
                GUILayout.EndVertical();

                GUI.color = Color.white;

                GUILayout.Box(string.Empty, GUILayout.Width(3), GUILayout.ExpandHeight(true));

                GUILayout.BeginVertical();
                {
                    GUILayout.Label(parameters.Count.ToString());

                    for (int i = 0; i < parameters.Count; i++)
                    {
                        RefreshParameter(parameters[i]);
                    }
                }
                GUILayout.EndVertical();

                GUI.color = Color.white;

                GUILayout.Box(string.Empty, GUILayout.Width(3), GUILayout.ExpandHeight(true));

                GUILayout.BeginVertical(GUILayout.Width(150));
                {
                    code = GUILayout.TextField(code);

                    if (GUILayout.Button("Build"))
                    {
                        CreateScript(code, parameters.ToArray());
                    }

                    if (GUILayout.Button("Reset Index"))
                    {
                        index = 0;
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshParameter(PropertyParameter parameter)
        {
            GUILayout.BeginHorizontal();
            {
                GUI.color = GetColor(parameter.property);

                parameter.old = EditorGUILayout.IntField(parameter.old, GUILayout.Width(30));

                if (parameter.order != parameter.old)
                {
                    parameter.order = parameter.old;

                    Sort();
                }

                parameter.variable = (VariableType)EditorGUILayout.EnumPopup(parameter.variable, GUILayout.Width(60));

                switch (parameter.variable)
                {
                    case VariableType.Array:
                    case VariableType.List:
                    case VariableType.Dictionary:
                        parameter.assistant = (VariableType)EditorGUILayout.EnumPopup(parameter.assistant, GUILayout.Width(60));
                        break;
                }

                parameter.name = GUILayout.TextField(parameter.name);

                parameter.description = GUILayout.TextField(parameter.description, GUILayout.Width(120));

                if (GUILayout.Button("-", GUILayout.Width(30)))
                {
                    int index = parameters.FindIndex(x => x.ID == parameter.ID);

                    if (index != -1)
                    {
                        parameters.RemoveAt(index);
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private void CreateScript(string code, params PropertyParameter[] parameters)
        {
            string[] path = NewScript(code);

            FileStream stream = new FileStream(path[0], FileMode.OpenOrCreate);

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
            content = string.Format("\tpublic class {0}", path[1]);

            content = string.Format("{0} : {1}", content, "MonoBehaviour");

            writer.WriteLine(content);
            writer.WriteLine("\t{");
            #endregion

            #region Attribute & Method
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
            #endregion

            writer.WriteLine("\t}");

            writer.WriteLine("}");

            writer.Dispose();

            stream.Dispose();

            AssetDatabase.Refresh();
        }

        /// <param name="writer"></param>
        /// <param name="parameter"></param>
        private void Write(ref StreamWriter writer, PropertyParameter parameter)
        {
            string content;

            if (!string.IsNullOrEmpty(parameter.description))
            {
                writer.WriteLine("\t\t/// <summary>");
                writer.WriteLine("\t\t/// " + parameter.description);
                writer.WriteLine("\t\t/// </summary>");
            }
            else
            {
                writer.WriteLine(string.Empty);
            }

            switch (parameter.property)
            {
                case PropertyType.Variable:
                    content = string.Format("\t\tpublic {0} {1};", parameter.Returned, parameter.name);
                    writer.WriteLine(content);
                    break;
                case PropertyType.Property:
                    string value = parameter.name.ToLowerInvariant();
                    content = string.Format("\t\tpublic {0} {1};", parameter.Returned, parameter.name.ToLowerInvariant());
                    writer.WriteLine(content);
                    content = string.Format("\t\tpublic {0} {1}", parameter.Returned, parameter.name.ToUpperInvariant());
                    writer.WriteLine(content);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\t" + string.Format("return {0};", value));
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t\tset");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\t" + string.Format("{0} = value;", value));
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t}");
                    break;
                case PropertyType.Method:
                    content = string.Format("\t\tpublic {0} {1}()", parameter.Returned, parameter.name);
                    writer.WriteLine(content);
                    writer.WriteLine("\t\t{");
                    switch (parameter.variable)
                    {
                        case VariableType.Bool:
                            writer.WriteLine("\t\t\treturn false;");
                            break;
                        case VariableType.Byte:
                        case VariableType.Int:
                        case VariableType.Float:
                        case VariableType.Double:
                            writer.WriteLine("\t\t\treturn 0;");
                            break;
                        case VariableType.String:
                            writer.WriteLine("\t\t\treturn string.Empty;");
                            break;
                        case VariableType.Array:
                        case VariableType.List:
                        case VariableType.Dictionary:
                            writer.WriteLine("\t\t\treturn null;");
                            break;
                        default:
                            writer.WriteLine(string.Empty);
                            break;
                    }
                    writer.WriteLine("\t\t}");
                    break;
            }
        }

        private void Sort()
        {
            parameters.Sort((a, b) =>
            {
                return a.order > b.order ? 1 : -1;
            });
        }

        private string[] NewScript(string code)
        {
            if (string.IsNullOrEmpty(code)) code = "NewScript";

            string folder = string.Format("{0}/Script/", Application.dataPath);

            string file = code;

            string path = string.Format("{0}{1}.cs", folder, file);

            int index = 0;

            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            if (File.Exists(path))
            {
                while (File.Exists(path))
                {
                    file = code + ++index;

                    path = string.Format("{0}{1}.cs", folder, file);
                }
            }

            return new string[2] { path, file };
        }

        private Color GetColor(PropertyType property)
        {
            switch (property)
            {
                case PropertyType.Variable:
                    return Color.green;
                case PropertyType.Property:
                    return Color.yellow;
                case PropertyType.Method:
                    return Color.grey;
                default:
                    return Color.white;
            }
        }


    }

    public class PropertyParameter
    {
        public string name;

        public int ID;

        public int old;

        public int order;

        public PropertyType property;

        public VariableType variable;

        public VariableType assistant;

        public string description;

        public string Returned
        {
            get
            {
                switch (variable)
                {
                    case VariableType.None:
                        return property switch
                        {
                            PropertyType.Method => "void",
                            _ => "string",
                        };
                    case VariableType.Array:
                        return string.Format("{0}[]", Variable(assistant));
                    case VariableType.List:
                        return string.Format("List<{0}>", Variable(assistant));
                    case VariableType.Dictionary:
                        return string.Format("Dictionary<int, {0}>", Variable(assistant));
                    default:
                        return Variable(variable);
                }
            }
        }

        private string Variable(VariableType variable)
        {
            switch (variable)
            {
                case VariableType.None:
                    return "string";
                case VariableType.Byte:
                    return "byte";
                case VariableType.Bool:
                    return "bool";
                case VariableType.Char:
                    return "char";
                case VariableType.Int:
                    return "int";
                case VariableType.Float:
                    return "float";
                case VariableType.Double:
                    return "double";
                case VariableType.String:
                    return "string";
                default:
                    goto case VariableType.None;
            }
        }
    }

    public enum PropertyType
    {
        Variable,           //变量
        Property,           //属性
        Method,             //方法
    }

    public enum VariableType
    {
        None,
        Bool,
        Byte,
        Char,
        Int,
        Float,
        Double,
        String,
        Array,
        List,
        Dictionary,
    }
}