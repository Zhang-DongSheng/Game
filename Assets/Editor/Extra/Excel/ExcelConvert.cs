using OfficeOpenXml;
using System.Text;

namespace UnityEditor
{
    public class ExcelConvert
	{
		private static int row, column;

		private static readonly StringBuilder builder = new StringBuilder();
		/// <summary>
		/// 转Json，一定要记得去掉最后一行的逗号, LitJson无法转译
		/// </summary>
		public static string ToJson(ExcelWorksheet sheet)
		{
			if (sheet == null) return string.Empty;

			var array = sheet.Cells.Value as object[,];

			row = array.GetLength(0);

			column = array.GetLength(1);

			builder.Clear();
			builder.Append("{\r\n");
			builder.Append("\"data\"");
			builder.Append(":\"");
			builder.Append(sheet.Name);
			builder.Append("\",\r\n");
			builder.Append("\"list\"");
			builder.Append(":");
			builder.Append("[\r\n");
			for (int i = 3; i < row; i++)
			{
				builder.Append("\t{\r\n");

				for (int j = 0; j < column; j++)
				{
					builder.Append("\t");
					builder.Append(string.Format("\"{0}\"", array[0, j]));
					builder.Append(":");
					string key = array[2, j].ToString().ToLower();
					switch (key)
					{
						case "int":
                        case "uint":
                        case "long":
						case "byte":
						case "float":
                        case "short":
                        case "double":
                            builder.Append(array[i, j]);
							break;
                        case "int[]":
                        case "uint[]":
                        case "float[]":
                        case "json":
                            builder.Append(array[i, j]);
                            break;
						default:
							builder.Append(string.Format("\"{0}\"", array[i, j]));
							break;
					}
					builder.Append(j + 1 == column ? "\r\n" : ",\r\n");
				}
				builder.Append(i + 1 == row ? "\t}\r\n" : "\t},\r\n");
			}
			builder.Append("]\r\n");
			builder.Append("}");
			return builder.ToString();
		}

		public static string ToXML(ExcelWorksheet sheet)
		{
			if (sheet == null) return string.Empty;

			var array = sheet.Cells.Value as object[,];

			row = array.GetLength(0);

			column = array.GetLength(1);

			builder.Clear();
			builder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			builder.Append("\r\n");
			builder.Append("<Table>");
			builder.Append("\r\n");
			for (int i = 3; i < row; i++)
			{
				builder.Append("\t<Row>");
				builder.Append("\r\n");
				for (int j = 0; j < column; j++)
				{
					builder.Append("\t\t<" + array[0, j].ToString() + ">");
					builder.Append(array[i, j].ToString());
					builder.Append("</" + array[0, j].ToString() + ">");
					builder.Append("\r\n");
				}
				builder.Append("\t</Row>");
				builder.Append("\r\n");
			}
			builder.Append("</Table>");

			return builder.ToString();
		}
	}
}