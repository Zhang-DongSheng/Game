using Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor.Script;

public class ExcelUtility
{
	private readonly DataSet m_dataSet;

	private DataTable m_dataTable;

	private readonly int m_tableCount;

	private int m_rowCount;

	private int m_columnCount;

	private readonly StringBuilder builder = new StringBuilder();

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

    #region Convert
    /// <summary>
    /// 转换为Json
    /// </summary>
    /// <param name="path">Json文件路径</param>
    /// <param name="Header">表头行数</param>
    public void ConvertToJson(string path, Encoding encoding)
	{
		//判断Excel文件中是否存在数据表
		if (m_tableCount < 1)
			return;

		//默认读取第一个数据表
		m_dataTable = m_dataSet.Tables[0];

		//读取数据表行数和列数
		m_rowCount = m_dataTable.Rows.Count;
		m_columnCount = m_dataTable.Columns.Count;

		//判断数据表内是否存在数据
		if (m_rowCount < 1 || m_columnCount < 1)
			return;

		//准备一个列表存储整个表的数据
		List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();

		//读取数据
		for (int i = 1; i < m_rowCount; i++)
		{
			//准备一个字典存储每一行的数据
			Dictionary<string, object> row = new Dictionary<string, object>();
			for (int j = 0; j < m_columnCount; j++)
			{
				//读取第1行数据作为表头字段
				string field = m_dataTable.Rows[0][j].ToString();
				//Key-Value对应
				row[field] = m_dataTable.Rows[i][j];
			}

			//添加到表数据中
			table.Add(row);
		}

		//生成Json字符串
		string json = JsonConvert.SerializeObject(table, Newtonsoft.Json.Formatting.Indented);

		/*絮大王添加代码：去掉所有的反斜杠+支持数组*/
		json = JsonSupportArray(json);

		Write(path, json, encoding);
	}

	/// <summary>
	/// 转换为CSV
	/// </summary>
	public void ConvertToCSV(string path, Encoding encoding)
	{
		if (m_tableCount < 1)
			return;

		m_dataTable = m_dataSet.Tables[0];

		m_rowCount = m_dataTable.Rows.Count;
		m_columnCount = m_dataTable.Columns.Count;

		if (m_rowCount < 1 || m_columnCount < 1)
			return;

		builder.Clear();

		for (int i = 0; i < m_rowCount; i++)
		{
			for (int j = 0; j < m_columnCount; j++)
			{
				//使用","分割每一个数值
				builder.Append(m_dataTable.Rows[i][j] + ",");
			}
			//使用换行符分割每一行
			builder.Append("\r\n");
		}

		Write(path, builder.ToString(), encoding);
	}

	/// <summary>
	/// 转换为Xml
	/// </summary>
	public void ConvertToXml(string path)
	{
		if (m_tableCount < 1)
			return;

		m_dataTable = m_dataSet.Tables[0];

		m_rowCount = m_dataTable.Rows.Count;
		m_columnCount = m_dataTable.Columns.Count;

		if (m_rowCount < 1 || m_columnCount < 1)
			return;

		this.builder.Clear();

		builder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
		builder.Append("\r\n");
		builder.Append("<Table>");
		builder.Append("\r\n");
		for (int i = 1; i < m_rowCount; i++)
		{
			builder.Append("  <Row>");
			builder.Append("\r\n");
			for (int j = 0; j < m_columnCount; j++)
			{
				builder.Append("   <" + m_dataTable.Rows[0][j].ToString() + ">");
				builder.Append(m_dataTable.Rows[i][j].ToString());
				builder.Append("</" + m_dataTable.Rows[0][j].ToString() + ">");
				builder.Append("\r\n");
			}
			builder.Append("  </Row>");
			builder.Append("\r\n");
		}
		builder.Append("</Table>");

		Write(path, builder.ToString(), Encoding.UTF8);
	}
	#endregion

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

	/// <summary>
	/// 设置目标实例的属性
	/// </summary>
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

	/// <summary>
	/// 写入文本
	/// </summary>
	/// <param name="path">文件路径</param>
	/// <param name="content">文本内容</param>
	/// <param name="encoding">编码格式</param>
	private void Write(string path, string content, Encoding encoding)
	{
		FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
		{
			TextWriter writer = new StreamWriter(stream, encoding);
			writer.Write(content);
			writer.Dispose();
		}
		stream.Dispose();
	}

    #region 絮大王添加的代码
    /// <summary>
    /// 让Json支持数组
    /// ————————————————————————————————————————————————————
    /// (这个方法是絮大王自己添加的)
    /// 此方法是为了让Excel在转化成Json的时候，支持数组  (格式为["元素1","元素2"])
    /// 
    /// * 为什么要这样做？
    ///   在源代码中，转化后的Json字符串是不支持数组的  (转化后，会变成这样：   "[\"元素1\",\"元素2\"]")
    /// 
    /// * 此方法的做了什么？
    ///   把Json字符串传递过来，就会返回一个支持数组的Json字符串  (格式为["元素1","元素2"])
    /// ——————————————————————————————————————————————————————
    /// </summary>
    /// <param name="jsonContent">json的文本内容</param>
    /// <returns></returns>
    private string JsonSupportArray(string jsonContent)
    {
        //去掉所有的反斜杠  （把"\"替换成""）
        jsonContent = jsonContent.Replace("\\", string.Empty);

        //为了能够支持数组，所以：
        //把所有的"[替换成[   
        //并且把所有的]"替换成]
        jsonContent = jsonContent.Replace("\"[", "[");
        jsonContent = jsonContent.Replace("]\"", "]");

        //把所有的".0" 替换成 ""
        //不然的话，"1"会显示为"1.0"
        jsonContent = jsonContent.Replace(".0,", ",");

        return jsonContent;
    }
    #endregion
}