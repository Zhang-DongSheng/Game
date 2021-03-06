﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor.Window;
using UnityEngine;

namespace UnityEditor
{
	public class ExcelTools : CustomWindow
	{
		private const string Extension = ".xlsx";

		private const string InputPath = "Source/Excel";

		private const string OutputPath = "Art/Excel";

		private readonly string[] text_view = new string[] { "Excel", "Setting", "Other" };

		private readonly string[] text_encoding = new string[] { "Default", "ASCII", "Unicode", "UTF-8", "UTF32", "GB2312" };

		private readonly string[] text_format = new string[] { "JSON", "CSV", "XML", "TXT" };

		private readonly string[] text_seacrch = new string[] { "Select", "Specify", "Auto" };

		private readonly string label_encoding = "Encoding";

		private readonly string label_format = "Format";

		private readonly string label_inputFolder = "Input Folder";

		private readonly string label_outputFolder = "Output Folder";

		private readonly string label_search = "Search";

		private readonly string label_create = "Create C#";

		private readonly string label_convert = "Convert";

		private readonly string label_revise = "Revise";

		private readonly string label_save = "Save";

		private int index_view;

		private int index_encoding;

		private int index_format;

		private int index_search;

		private Rect rect_inputFolder;

		private Rect rect_outputFolder;

		private string input_inputFolder;

		private string input_outputFolder;

		private Encoding encoding;

		private int search;

		private string inputFolder;

		private string outputFolder;

		private readonly List<ItemFile> source = new List<ItemFile>();

		protected override string Title { get { return "Excel"; } }
		[MenuItem("Art/Excel")]
		protected static void Open()
		{
			Open<ExcelTools>();
		}

		protected override void Init()
		{
			input_inputFolder = Path.Combine(Application.dataPath.Remove(Application.dataPath.Length - 6, 6), InputPath);

			inputFolder = input_inputFolder;

			input_outputFolder = Path.Combine(Application.dataPath, OutputPath);

			outputFolder = input_outputFolder;

			encoding = Encoding.Default;

			Load();
		}

		protected override void Refresh()
		{
			index_view = GUILayout.Toolbar(index_view, text_view);

			GUILayout.BeginArea(new Rect(20, 30, Screen.width - 40, Screen.height - 50));
			{
				switch (index_view)
				{
					case 0:
						RefreshUIExcel();
						break;
					case 1:
						RefreshUISetting();
						break;
					default:
						RefreshUIOther();
						break;
				}
			}
			GUILayout.EndArea();
		}

		private void RefreshUIExcel()
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label(label_search, GUILayout.Width(100));

				index_search = EditorGUILayout.Popup(index_search, text_seacrch);

				if (index_search != search)
				{
					search = index_search;

					Load();
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				if (search == 1)
				{
					GUILayout.Label(label_inputFolder, GUILayout.Width(100));

					rect_inputFolder = EditorGUILayout.GetControlRect(GUILayout.Width(Screen.width - 250));

					input_inputFolder = EditorGUI.TextField(rect_inputFolder, input_inputFolder);

					if ((Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragExited) && rect_inputFolder.Contains(Event.current.mousePosition))
					{
						DragAndDrop.visualMode = DragAndDropVisualMode.Generic;

						if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
						{
							input_inputFolder = Application.dataPath + DragAndDrop.paths[0].Remove(0, 6);
						}
					}

					if (GUILayout.Button(label_revise, GUILayout.Width(100)))
					{
						inputFolder = input_inputFolder; Load();
					}
				}
			}
			GUILayout.EndHorizontal();

			if (source.Count > 0)
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.BeginVertical();
					{
						GUILayout.BeginHorizontal();
						{
							GUILayout.Label("S", GUILayout.Width(20));
							GUILayout.Label("Name", GUILayout.Width(120));
							GUILayout.Label("Path");
						}
						GUILayout.EndHorizontal();

						scroll = GUILayout.BeginScrollView(scroll);
						{
							for (int i = 0; i < source.Count; i++)
							{
								GUILayout.BeginHorizontal();
								{
									source[i].select = GUILayout.Toggle(source[i].select, string.Empty, GUILayout.Width(20));
									GUILayout.Label(source[i].name, GUILayout.Width(120));
									GUILayout.Label(source[i].path);
								}
								GUILayout.EndHorizontal();
							}
						}
						GUILayout.EndScrollView();
					}
					GUILayout.EndVertical();

					GUILayout.Space(15);

					GUILayout.BeginVertical(GUILayout.Width(100));
					{
						GUILayout.Space(20);

						if (GUILayout.Button(label_create))
						{
							Create();
						}

						if (GUILayout.Button(label_convert))
						{
							Convert();
						}

						if (GUILayout.Button(label_inputFolder))
						{
							OpenFolder(inputFolder);
						}

						if (GUILayout.Button(label_outputFolder))
						{
							OpenFolder(outputFolder);
						}
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndHorizontal();
			}
			else
			{
				EditorGUILayout.LabelField("Source is Empty!");
			}

			GUILayout.Space(10);
		}

		private void RefreshUISetting()
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label(label_format, GUILayout.Width(100));

				index_format = EditorGUILayout.Popup(index_format, text_format);
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				GUILayout.Label(label_encoding, GUILayout.Width(100));

				index_encoding = EditorGUILayout.Popup(index_encoding, text_encoding);

				if (GUILayout.Button(label_save, GUILayout.Width(100)))
				{
					switch (index_encoding)
					{
						case 0:
							encoding = Encoding.Default;
							break;
						case 1:
							encoding = Encoding.ASCII;
							break;
						case 2:
							encoding = Encoding.Unicode;
							break;
						case 3:
							encoding = Encoding.UTF8;
							break;
						case 4:
							encoding = Encoding.UTF32;
							break;
						case 5:
							encoding = Encoding.GetEncoding("GB2312");
							break;
						default:
							encoding = Encoding.Default;
							break;
					}
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				GUILayout.Label(label_outputFolder, GUILayout.Width(100));

				rect_outputFolder = EditorGUILayout.GetControlRect(GUILayout.Width(Screen.width - 247));

				input_outputFolder = EditorGUI.TextField(rect_outputFolder, input_outputFolder);

				if ((Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragExited) && rect_outputFolder.Contains(Event.current.mousePosition))
				{
					DragAndDrop.visualMode = DragAndDropVisualMode.Generic;

					if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
					{
						input_outputFolder = Application.dataPath + DragAndDrop.paths[0].Remove(0, 6);
					}
				}

				if (GUILayout.Button(label_save, GUILayout.Width(100)))
				{
					outputFolder = input_outputFolder;
				}
			}
			GUILayout.EndHorizontal();
		}

		private void RefreshUIOther()
		{
			if (GUILayout.Button("联系我们"))
			{
				Application.OpenURL("https://www.baidu.com");
			}
		}

		private void OnSelectionChange()
		{
			Show(); Load(); Repaint();
		}

		private void Load()
		{
			source.Clear();

			switch (search)
			{
				case 0:
					object[] selection = (object[])Selection.objects;

					string path;

					if (selection != null && selection.Length > 0)
					{
						foreach (UnityEngine.Object asset in selection)
						{
							path = AssetDatabase.GetAssetPath(asset);

							if (path.EndsWith(Extension))
							{
								source.Add(new ItemFile()
								{
									name = FileName(path, Extension),
									path = Application.dataPath + path.Remove(0, 6),
									select = true,
								});
							}
						}
					}
					break;
				case 1:
					if (!string.IsNullOrEmpty(inputFolder) && Directory.Exists(inputFolder))
					{
						DirectoryInfo root = new DirectoryInfo(inputFolder);

						foreach (FileInfo file in root.GetFiles())
						{
							if (file.Extension.Equals(Extension))
							{
								source.Add(new ItemFile()
								{
									name = FileName(file.Name, Extension),
									path = file.FullName,
									select = true,
								});
							}
						}
					}
					break;
				default:
					List<string> searchResult = Find(Application.dataPath, Extension);

					if (searchResult.Count > 0)
					{
						foreach (string file in searchResult)
						{
							source.Add(new ItemFile()
							{
								name = FileName(file, Extension),
								path = file,
								select = true,
							});
						}
					}
					break;
			}
		}

		private void Create()
		{
			try
			{
				for (int i = 0; i < source.Count; i++)
				{
					if (source[i].select)
					{
						new ExcelUtility(source[i].path).CreateScript();
					}
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message);
			}
			finally
			{

			}
		}

		private void Convert()
		{
			if (!Directory.Exists(outputFolder))
				Directory.CreateDirectory(outputFolder);

			for (int i = 0; i < source.Count; i++)
			{
				if (source[i].select)
				{
					try
					{
						ExcelUtility excel = new ExcelUtility(source[i].path);

						switch (index_format)
						{
							case 0:
								excel.ConvertToJson(string.Format("{0}/{1}.json", outputFolder, source[i].name), encoding);
								break;
							case 1:
								excel.ConvertToCSV(string.Format("{0}/{1}.csv", outputFolder, source[i].name), encoding);
								break;
							case 2:
								excel.ConvertToXml(string.Format("{0}/{1}.xml", outputFolder, source[i].name));
								break;
							case 3:
								excel.ConvertToJson(string.Format("{0}/{1}.txt", outputFolder, source[i].name), encoding);
								break;
							default:

								break;
						}
					}
					catch (Exception e)
					{
						Debug.LogError(e.Message);
					}
					finally
					{

					}
				}
				AssetDatabase.Refresh();
			}
		}

		private List<string> Find(string path, string suffix)
		{
			List<string> result = new List<string>();

			Find(path, suffix, ref result);

			return result;
		}

		private void Find(string path, string suffix, ref List<string> result)
		{
			if (Directory.Exists(path))
			{
				DirectoryInfo root = new DirectoryInfo(path);

				foreach (FileInfo file in root.GetFiles())
				{
					if (file.Extension.Equals(suffix))
					{
						result.Add(file.FullName);
					}
				}

				string[] dirs = Directory.GetDirectories(path);

				if (dirs.Length > 0)
				{
					foreach (string dir in dirs)
					{
						Find(dir, suffix, ref result);
					}
				}
			}
		}

		private string FileName(string path, string extension)
		{
			string[] param = path.Split('/', '\\');

			if (param != null && param.Length > 0)
			{
				path = param[param.Length - 1];
			}

			if (path.EndsWith(extension))
			{
				path = path.Remove(path.Length - extension.Length, extension.Length);
			}

			return path;
		}

		private void OpenFolder(string path)
		{
			if (string.IsNullOrEmpty(path)) return;

			if (Directory.Exists(path))
			{
				path = path.Replace("/", "\\");

				System.Diagnostics.Process.Start("explorer.exe", path);
			}
			else
			{
				Debug.LogError("No Directory: " + path);
			}
		}
	}
}