using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
    public class PsdTools : EditorWindow
	{
		private const string Extension = ".psd";

		private const string InputPath = "Source/PSD";

		private const string OutputPath = "Art/PSD";

		private readonly string[] text_view = new string[] { "PSD", "Setting", "Other" };

		private readonly string[] text_seacrch = new string[] { "Select", "Specify", "Auto" };

		private readonly string label_inputFolder = "Input Folder";

		private readonly string label_outputFolder = "Output Folder";

		private readonly string label_search = "Search";

		private readonly string label_convert = "Convert";

		private readonly string label_select = "Select ";

		private readonly string label_revise = "Revise";

		private readonly string label_save = "Save";

		private int index_view;

		private int index_search;

		private int index_searchNode;

		private Rect rect_inputFolder;

		private Rect rect_outputFolder;

		private string input_inputFolder;

		private string input_outputFolder;

		private Vector2 scroll;

		private int search;

		private int searchNode;

		private string inputFolder;

		private string outputFolder;

		private readonly List<string> node = new List<string>();

		private readonly List<ItemFile> source = new List<ItemFile>();

		[MenuItem("Data/PSD")]
		private static void Open()
		{
			PsdTools window = EditorWindow.GetWindow<PsdTools>();
			window.titleContent = new GUIContent("PSD Tools");
			window.minSize = new Vector2(500, 200);
			window.Init();
			window.Show();
		}

		public void Init()
		{
			string source = Application.dataPath.Remove(Application.dataPath.Length - 6, 6);

			input_inputFolder = Path.Combine(source, InputPath);

			inputFolder = input_inputFolder;

			input_outputFolder = OutputPath;

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

					rect_inputFolder = EditorGUILayout.GetControlRect(GUILayout.Width(Screen.width - 247));

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
						inputFolder = input_inputFolder;

						LoadSource();
					}
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				if (search == 1 && node != null && node.Count > 0)
				{
					GUILayout.Label(label_inputFolder, GUILayout.Width(100));

					index_searchNode = EditorGUILayout.Popup(index_searchNode, node.ToArray());

					if (index_searchNode != searchNode)
					{
						searchNode = index_searchNode;

						LoadSource();
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

						if (GUILayout.Button(label_convert, GUILayout.Height(40)))
						{
							Convert();
						}

						if (GUILayout.Button(label_select + "All"))
						{
							Select(true);
						}

						if (GUILayout.Button(label_select + "None"))
						{
							Select(false);
						}

						if (GUILayout.Button(label_inputFolder))
						{
							OpenFolder(inputFolder);
						}

						if (GUILayout.Button(label_outputFolder))
						{
							OpenFolder(string.Format("{0}/{1}", Application.dataPath, outputFolder));
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

			string path;

			switch (search)
			{
				case 0:
					object[] selection = (object[])Selection.objects;

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
					node.Clear();

					if (!string.IsNullOrEmpty(inputFolder) && Directory.Exists(inputFolder))
					{
						DirectoryInfo root = new DirectoryInfo(inputFolder);

						foreach (DirectoryInfo dir in root.GetDirectories())
						{
							node.Add(dir.Name);
						}

						if (node.Count > searchNode) { }
						else
						{
							index_searchNode = searchNode = 0;
						}
					}

					string folderName = node.Count > searchNode ? node[searchNode] : string.Empty;

					path = string.Format("{0}/{1}", inputFolder, folderName);

					if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
					{
						DirectoryInfo root = new DirectoryInfo(path);

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

		private void Select(bool select)
		{
			for (int i = 0; i < source.Count; i++)
			{
				source[i].select = select;
			}
		}

		private void Convert()
		{
			string folderName = node.Count > index_searchNode ? node[index_searchNode] : string.Empty;

			string path = string.Format("{0}/{1}/{2}", Application.dataPath, outputFolder, folderName);

			for (int i = 0; i < source.Count; i++)
			{
				if (source[i].select)
				{
                    try
                    {
						Pfi.PfiImporter.AutoImport(source[i].path, string.Format("{0}/{1}", path, source[i].name));
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