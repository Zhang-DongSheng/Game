using Pfi;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
    public class PSDTools : EditorWindow
	{
		private const string Extension = ".psd";

		private const string InputPath = "Source/PSD/Input";

		private const string OutputPath = "Source/PSD/Output";

		private readonly string[] text_view = new string[] { "PSD", "Setting", "Other" };

		private readonly string[] text_seacrch = new string[] { "Select", "Specify", "Auto" };

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

		private Vector2 scroll;

		private int search;

		private string inputFolder;

		private string outputFolder;

		private readonly List<FileItem> source = new List<FileItem>();

		[MenuItem("Data/PSD")]
		private static void Open()
		{
			PSDTools window = EditorWindow.GetWindow<PSDTools>();
			window.titleContent = new GUIContent("PSD Tools");
			window.minSize = new Vector2(500, 200);
			window.Init();
			window.Show();
		}

		public void Init()
		{
			input_inputFolder = Path.Combine(Application.dataPath, InputPath);

			inputFolder = input_inputFolder;

			input_outputFolder = Path.Combine(Application.dataPath, OutputPath);

			outputFolder = input_outputFolder;

			LoadSource();
		}

		private void OnSelectionChange()
		{
			Show(); LoadSource(); Repaint();
		}

		private void OnGUI()
		{
			index_view = GUILayout.Toolbar(index_view, text_view);

			GUILayout.BeginArea(new Rect(20, 30, Screen.width - 40, Screen.height - 50));
			{
				switch (index_view)
				{
					case 0:
						RefreshUIPSD();
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

		private void RefreshUIPSD()
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label(label_search, GUILayout.Width(100));

				index_search = EditorGUILayout.Popup(index_search, text_seacrch);

				if (index_search != search)
				{
					search = index_search;

					LoadSource();
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
						inputFolder = input_inputFolder; LoadSource();
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
									source[i].output = GUILayout.Toggle(source[i].output, string.Empty, GUILayout.Width(20));
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

		private void LoadSource()
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
								source.Add(new FileItem()
								{
									name = FileName(path, Extension),
									path = Application.dataPath + path.Remove(0, 6),
									output = true,
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
								source.Add(new FileItem()
								{
									name = FileName(file.Name, Extension),
									path = file.FullName,
									output = true,
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
							source.Add(new FileItem()
							{
								name = FileName(file, Extension),
								path = file,
								output = true,
							});
						}
					}
					break;
			}
		}

		private void Convert()
		{
			if (!Directory.Exists(outputFolder))
				Directory.CreateDirectory(outputFolder);

			for (int i = 0; i < source.Count; i++)
			{
				if (source[i].output)
				{
					try
					{
						PfiImporter.AutoImport(source[i].path, outputFolder);
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