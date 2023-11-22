using Game;
using System;
using System.Collections.Generic;
using System.IO;
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

		private readonly string[] text_format = new string[] { "JSON", "CSV", "XML", "TXT" };

		private readonly string[] text_seacrch = new string[] { "Select", "Specify", "Auto" };

		private int index_view;

		private int index_format;

		private int index_search;

		private Rect rect_inputFolder;

		private Rect rect_outputFolder;

		private string input_inputFolder;

		private string input_outputFolder;

		private int search;

		private string inputFolder;

		private string outputFolder;

		private readonly List<ItemFile> source = new List<ItemFile>();

		[MenuItem("Extra/Excel")]
		protected static void Open()
		{
			Open<ExcelTools>("表格工具");
		}

		protected override void Initialise()
		{
			input_inputFolder = Path.Combine(Utility.Path.Project, InputPath);

			inputFolder = input_inputFolder;

			input_outputFolder = Path.Combine(Application.dataPath, OutputPath);

			outputFolder = input_outputFolder;

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
				GUILayout.Label("搜索", GUILayout.Width(100));

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
					GUILayout.Label("输入", GUILayout.Width(100));

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

					if (GUILayout.Button(ToLanguage("Refresh"), GUILayout.Width(100)))
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

					GUILayout.BeginVertical(GUILayout.Width(120));
					{
						GUILayout.Space(20);

						if (GUILayout.Button(ToLanguage("Build")))
						{
							CreateAsset();
						}
						GUILayout.BeginHorizontal();
						{
							index_format = EditorGUILayout.Popup(index_format, text_format);

							if (GUILayout.Button(ToLanguage("Convert")))
							{
								Convert();
							}
						}
						GUILayout.EndHorizontal();

						if (GUILayout.Button(ToLanguage("Input Folder")))
						{
							OpenFolder(inputFolder);
						}
						if (GUILayout.Button(ToLanguage("Output Folder")))
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
				GUILayout.Label("输出", GUILayout.Width(100));

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

				if (GUILayout.Button(ToLanguage("Save"), GUILayout.Width(100)))
				{
					outputFolder = input_outputFolder;
				}
			}
			GUILayout.EndHorizontal();
		}

		private void RefreshUIOther()
		{
			if (GUILayout.Button(ToLanguage("联系我们")))
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

		private void CreateAsset()
		{
			try
			{
				for (int i = 0; i < source.Count; i++)
				{
					if (source[i].select)
					{
						new ExcelUtility(source[i].path).CreateAsset();
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
			if (Directory.Exists(outputFolder)) { }
			else
			{
				Directory.CreateDirectory(outputFolder);
			}
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
								excel.ConvertToJson(string.Format("{0}/{1}.json", outputFolder, source[i].name));
								break;
							case 1:
								excel.ConvertToCSV(string.Format("{0}/{1}.csv", outputFolder, source[i].name));
								break;
							case 2:
								excel.ConvertToXml(string.Format("{0}/{1}.xml", outputFolder, source[i].name));
								break;
							case 3:
								excel.ConvertToJson(string.Format("{0}/{1}.txt", outputFolder, source[i].name));
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