using Excel;
using System.Data;
using System.IO;
using System.Text;

namespace UnityEditor
{
    public class ExcelUtility
	{
		private readonly DataSet m_dataSet;

		private readonly Encoding UTF8 = new UTF8Encoding(false);

		public ExcelUtility(string path)
		{
			if (File.Exists(path))
			{
				using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read))
				{
					IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

					m_dataSet = reader.AsDataSet();
				};
			}
			else
			{
				UnityEngine.Debug.LogError("Path Error: " + path);
			}
		}

		public void ConvertToJson(string path)
		{
			string content = ExcelConvert.ToJson(m_dataSet.Tables[0]);

			File.WriteAllText(path, content, UTF8);
		}

		public void ConvertToCSV(string path)
		{
			string content = ExcelConvert.ToCSV(m_dataSet.Tables[0]);

			File.WriteAllText(path, content, UTF8);
		}

		public void ConvertToXml(string path)
		{
			string content = ExcelConvert.ToXML(m_dataSet.Tables[0]);

			File.WriteAllText(path, content, UTF8);
		}

		public void CreateAsset()
		{
			ExcelConvert.CreateAsset(m_dataSet.Tables[0]);
		}
	}
}