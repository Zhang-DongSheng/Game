using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using Utils;

public class ExcelUtility
{
	private readonly DataSet m_dataSet;

	private DataTable m_dataTable;

	private readonly int m_tableCount;

	private int m_rowCount;

	private int m_columnCount;

	private readonly Encoding UTF8 = new UTF8Encoding(false);

	public ExcelUtility (string path)
	{
		if (File.Exists(path))
		{
			using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read))
			{
				IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

				m_dataSet = reader.AsDataSet();

				m_tableCount = m_dataSet != null ? m_dataSet.Tables.Count : 0;
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

	/// <summary>
	/// 创建实体类
	/// </summary>
	public void CreateScript()
	{
		if (m_tableCount < 1)
			return;

		m_dataTable = m_dataSet.Tables[0];

		m_rowCount = m_dataTable.Rows.Count;
		m_columnCount = m_dataTable.Columns.Count;

		if (m_rowCount < 1 || m_columnCount < 1)
			return;

		//AutomationScriptWindow.CreateDataScript(m_dataTable.TableName, m_dataTable);
	}

    /// <summary>
    /// 转换为实体类列表
    /// </summary>
    public List<T> ConvertToList<T>()
	{
		//判断Excel文件中是否存在数据表
		if (m_dataSet.Tables.Count < 1)
			return null;
		//默认读取第一个数据表
		DataTable mSheet = m_dataSet.Tables[0];

		//判断数据表内是否存在数据
		if (mSheet.Rows.Count < 1)
			return null;

		//读取数据表行数和列数
		int rowCount = mSheet.Rows.Count;
		int colCount = mSheet.Columns.Count;

		//准备一个列表以保存全部数据
		List<T> list = new List<T>();

		//读取数据
		for (int i = 1; i < rowCount; i++)
		{
			//创建实例
			Type t = typeof(T);
			ConstructorInfo ct = t.GetConstructor(System.Type.EmptyTypes);
			T target = (T)ct.Invoke(null);
			for (int j = 0; j < colCount; j++)
			{
				//读取第1行数据作为表头字段
				string field = mSheet.Rows[0][j].ToString();
				object value = mSheet.Rows[i][j];
				//设置属性值
				SetTargetProperty(target, field, value);
			}

			//添加至列表
			list.Add(target);
		}

		return list;
	}

	private void SetTargetProperty(object target, string propertyName, object propertyValue)
	{
		//获取类型
		Type mType = target.GetType();
		//获取属性集合
		PropertyInfo[] mPropertys = mType.GetProperties();
		foreach (PropertyInfo property in mPropertys)
		{
			if (property.Name == propertyName)
			{
				property.SetValue(target, Convert.ChangeType(propertyValue, property.PropertyType), null);
			}
		}
	}
}