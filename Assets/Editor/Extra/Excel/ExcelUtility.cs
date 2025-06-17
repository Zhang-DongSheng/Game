using OfficeOpenXml;
using System.IO;
using System.Linq;
using System.Text;

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
				}
			}
			else
			{
				UnityEngine.Debuger.LogError(UnityEngine.Author.Editor, "Path Error: " + path);
			}
		}

		public void ConvertToJson(string path)
		{
			var sheet = m_sheets.FirstOrDefault();

			string content = ExcelConvert.ToJson(sheet);

			File.WriteAllText(path, content, UTF8);
		}

		public void ConvertToXml(string path)
		{
			var sheet = m_sheets.FirstOrDefault();

			string content = ExcelConvert.ToXML(sheet);

			File.WriteAllText(path, content, UTF8);
		}

		public void Dispose()
		{
			m_sheets.Dispose();
        }
	}
}