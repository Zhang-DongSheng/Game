using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Utils
{
    public class ExcelConvert
	{
		private static int row, column;

		private static readonly List<ExcelItem> list = new List<ExcelItem>();

		private static readonly StringBuilder builder = new StringBuilder();

		public static void CreateCSharp(DataTable table)
		{
			if (!Enter(table)) return;

			ToList(table);
		}

		public static string ToJson(DataTable table)
		{
			if (!Enter(table)) return string.Empty;

			ToList(table);

			builder.Clear();
			builder.Append("{\r\n");
			builder.Append("'data'");
			builder.Append(":");
			builder.Append(table.TableName);
			builder.Append(",\r\n");
			builder.Append("'list'");
			builder.Append(":");
			builder.Append("[\r\n");
			for (int i = 0; i < list.Count; i++)
			{
				builder.Append("{\r\n");

				for (int j = 0; i < list[i].list.Count; j++)
				{
					builder.Append(string.Format("'{0}'", list[i].name));
					builder.Append(":");
					builder.Append(string.Format("'{0}'", list[i].list[j]));
					builder.Append(",\r\n");
				}
				builder.Append("},\r\n");
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
			for (int i = 1; i < row; i++)
			{
				builder.Append("  <Row>");
				builder.Append("\r\n");
				for (int j = 0; j < column; j++)
				{
					builder.Append("   <" + table.Rows[0][j].ToString() + ">");
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

			for (int i = 0; i < column; i++)
			{
				ExcelItem item = new ExcelItem()
				{
					name = table.Rows[0][i].ToString(),
					type = table.Rows[1][i].ToString(),
					list = new List<object>(row - 2)
				};
				for (int j = 2; j < row; j++)
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

		struct ExcelItem
		{
			public string name;

			public string type;

			public List<object> list;
		}
	}
}