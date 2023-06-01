using OfficeOpenXml;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEngine;

namespace UnityEditor
{
    public class ExcelUtility
	{
		private readonly ExcelWorksheets m_sheets;

		private readonly Encoding UTF8 = new UTF8Encoding(false);

		public ExcelUtility(string path)
		{
			if (File.Exists(path))
			{
				using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read))
				{
					ExcelPackage package = new ExcelPackage(stream);

					m_sheets = package.Workbook.Worksheets;
				};
			}
			else
			{
				UnityEngine.Debug.LogError("Path Error: " + path);
			}
		}

		public void ConvertToJson(string path)
		{
			foreach (var sheet in m_sheets)
			{
                string content = ExcelConvert.ToJson(sheet);

                File.WriteAllText(path, content, UTF8);
            }
		}

		public void ConvertToCSV(string path)
		{
			string content = ExcelConvert.ToCSV(null);

			File.WriteAllText(path, content, UTF8);
		}

		public void ConvertToXml(string path)
		{
			string content = ExcelConvert.ToXML(null);

			File.WriteAllText(path, content, UTF8);
		}

		public void CreateAsset()
		{
			ExcelConvert.CreateAsset(null);
		}
	}
}