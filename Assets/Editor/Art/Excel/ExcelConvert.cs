using Data;
using Game;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace UnityEditor
{
    public class ExcelConvert
	{
		private static int row, column;

		private static readonly List<ExcelColumn> list = new List<ExcelColumn>();

		private static readonly StringBuilder builder = new StringBuilder();

        public static void CreateAsset(DataTable table)
        {
            if (!Enter(table)) return;

            if (CreateOrUpdateCSharp(table))
            {
                Debug.LogError("Create Scripte! Please Try Again!!!");
                return;
            }
            try
            {
                string script = string.Format("Data{0}", table.TableName);

                string path = string.Format("Assets/Package/Data/{0}.asset", script);

                Assembly assembly = typeof(DataBase).Assembly;

                Type TA = assembly.GetType(string.Format("Data.Data{0}", table.TableName));

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                ScriptableObject asset = ScriptableObject.CreateInstance(script);

                asset.name = script;

                Type TI = assembly.GetType(string.Format("Data.{0}Information", table.TableName));

                object list = TI.GenerateCollection();

                for (int i = 2; i < row; i++)
                {
                    object target = Activator.CreateInstance(TI);

                    for (int j = 0; j < column; j++)
                    {
                        var field = TI.GetField(table.Rows[0][j].ToString(), BindingFlags.Instance | BindingFlags.Public);

                        object value;

                        switch (table.Rows[1][j].ToString())
                        {
                            case "int[]":
                                {
                                    if (!table.Rows[i][j].ToString().TryParseArrayInt(out int[] array_int))
                                    {

                                    }
                                    value = array_int;
                                }
                                break;
                            case "float[]":
                                {
                                    if (!table.Rows[i][j].ToString().TryParseArrayFloat(out float[] array_float))
                                    {

                                    }
                                    value = array_float;
                                }
                                break;
                            case "DateTime":
                                {
                                    if (!table.Rows[i][j].ToString().TryParseDateTime(out DateTime time))
                                    {

                                    }
                                    value = time;
                                }
                                break;
                            default:
                                value = Convert.ChangeType(table.Rows[i][j], field.FieldType);
                                break;
                        }
                        field.SetValue(target, value);
                    }
                    list.Call("Add", new object[] { target });
                }
                asset.SetMember("list", list);

                AssetDatabase.CreateAsset(asset, path);

                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.Data, e);
            }
        }

		public static bool CreateOrUpdateCSharp(DataTable table)
		{
			if (!Enter(table)) return true;

			ToList(table);

			string path = string.Format("{0}/Script/Data/Assets/Data{1}.cs", Application.dataPath, table.TableName);

			if (File.Exists(path))
			{
				Assembly assembly = typeof(DataBase).Assembly;

				Type T = assembly.GetType(string.Format("Data.{0}Information", table.TableName));

				bool modify = false;

				for (int i = 0; i < list.Count; i++)
				{
					if (T.GetField(list[i].name) == null || T.GetField(list[i].name).FieldType.Name != CodeUtils.OfficialName(list[i].type))
					{
						Debuger.LogWarning(Author.Data, $"Type: {T.GetField(list[i].name).FieldType.Name} != {CodeUtils.OfficialName(list[i].type)}");
						modify = true;
						break;
					}
				}
				if (modify)
					CreateCSharp(list, path, table.TableName);
				else
					return false;
			}
			else
			{
				CreateCSharp(list, path, table.TableName);
			}
			return true;
		}

		public static void CreateCSharp(List<ExcelColumn> columns, string path, string name)
		{
			builder.Clear();
			builder.AppendLine("using UnityEngine;");
			builder.AppendLine("using System.Collections.Generic;");
			builder.AppendLine("using Game;");
			builder.AppendLine();
			builder.AppendLine("namespace Data");
			builder.AppendLine("{");
			builder.AppendLine(string.Format("\tpublic class Data{0} : DataBase", name));
			builder.AppendLine("\t{");
			builder.AppendLine(string.Format("\t\tpublic List<{0}Information> list;", name));
			builder.AppendLine("\t}");
			builder.AppendLine("\t[System.Serializable]");
			builder.AppendLine(string.Format("\tpublic class {0}Information", name));
			builder.AppendLine("\t{");
			for (int i = 0; i < columns.Count; i++)
			{
				if (i > 0) builder.AppendLine();
				builder.Append("\t\tpublic");
				builder.Append(" ");
				if (string.IsNullOrEmpty(columns[i].type))
				{
					builder.Append("object");
				}
				else
				{
					switch (columns[i].type)
					{
						default:
							builder.Append(columns[i].type);
							break;
					}
				}
				builder.Append(" ");
				builder.Append(columns[i].name);
				builder.AppendLine(";");
			}
			builder.AppendLine("\t}");
			builder.AppendLine("}");

			if (File.Exists(path))
			{
				File.Delete(path);
			}
			File.WriteAllText(path, builder.ToString(), new UTF8Encoding(false));

			AssetDatabase.Refresh();
		}

		public static string ToJson(DataTable table)
		{
			if (!Enter(table)) return string.Empty;

			builder.Clear();
			builder.Append("{\r\n");
			builder.Append("\"data\"");
			builder.Append(":\"");
			builder.Append(table.TableName);
			builder.Append("\",\r\n");
			builder.Append("\"list\"");
			builder.Append(":");
			builder.Append("[\r\n");
			for (int i = 2; i < row; i++)
			{
				builder.Append("\t{\r\n");
				for (int j = 0; j < column; j++)
				{
					builder.Append("\t");
					builder.Append(string.Format("\"{0}\"", table.Rows[0][j]));
					builder.Append(":");
					builder.Append(string.Format("\"{0}\"", table.Rows[i][j]));
					builder.Append(",\r\n");
				}
				builder.Append("\t},\r\n");
			}
			builder.Append("]\r\n");
			builder.Append("}");
			return builder.ToString();
		}

		public static string ToXML(DataTable table)
		{
			if (!Enter(table)) return string.Empty;

			builder.Clear();
			builder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			builder.Append("\r\n");
			builder.Append("<Table>");
			builder.Append("\r\n");
			for (int i = 2; i < row; i++)
			{
				builder.Append("\t<Row>");
				builder.Append("\r\n");
				for (int j = 0; j < column; j++)
				{
					builder.Append("\t\t<" + table.Rows[0][j].ToString() + ">");
					builder.Append(table.Rows[i][j].ToString());
					builder.Append("</" + table.Rows[0][j].ToString() + ">");
					builder.Append("\r\n");
				}
				builder.Append("  </Row>");
				builder.Append("\r\n");
			}
			builder.Append("</Table>");

			return builder.ToString();
		}

		public static string ToCSV(DataTable table)
		{
			if (!Enter(table)) return string.Empty;

			for (int i = 0; i < row; i++)
			{
				for (int j = 0; j < column; j++)
				{
					builder.Append(table.Rows[i][j] + ",");
				}
				builder.Append("\r\n");
			}
			return builder.ToString();
		}

		private static void ToList(DataTable table)
		{
			list.Clear();

			for (int i = 2; i < column; i++)
			{
				ExcelColumn item = new ExcelColumn()
				{
					name = table.Rows[0][i].ToString(),
					type = table.Rows[1][i].ToString(),
					list = new List<object>(row - 2)
				};

				for (int j = 2; j < column; j++)
				{
					item.list.Add(table.Rows[j][i]);
				}
				list.Add(item);
			}
		}

		private static bool Enter(DataTable table)
		{
			if (table == null) return false;

			row = table.Rows.Count;

			column = table.Columns.Count;

			return row > 2 && column > 1;
		}

		public struct ExcelColumn
		{
			public string name;

			public string type;

			public List<object> list;
		}
	}
}