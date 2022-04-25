using Game;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
	public class PsdTools : EditorWindow
	{
		private const string Extension = ".psd";

		private const string InputPath = "Source/Psd";

		private const string OutputPath = "Art/PSD";

		private readonly string[] text_view = new string[] { "PSD", "设置", "其他" };

		private readonly string[] text_seacrch = new string[] { "选择", "指定目录", "检索游戏内资源" };

		private readonly string label_inputFolder = "输入：";

		private readonly string label_outputFolder = "输出：";

		private int index_view;

		private int index_search;

		private int index_searchNode;

		private string input_inputFolder;

		private string input_outputFolder;

		private Vector2 scroll;

		private int search;

		private int searchNode;

		private bool assetbundle;

		private string inputFolder;

		private string outputFolder;

		private readonly List<string> node = new List<string>();

		private readonly List<FileItem> source = new List<FileItem>();

		[MenuItem("Art/PSD")]
		protected static void Open()
		{
			PsdTools window = EditorWindow.GetWindow<PsdTools>();
			window.titleContent = new GUIContent("PSD 导出工具");
			window.minSize = new Vector2(500, 200);
			window.Init();
			window.Show();
		}

		public void Init()
		{
			input_inputFolder = Path.Combine(Utility.Path.Project, InputPath);

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
				GUILayout.Label("搜索：", GUILayout.Width(100));

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

					if (GUILayout.Button(input_inputFolder))
					{
						input_inputFolder = EditorUtility.SaveFolderPanel("输出文件夹", input_inputFolder, string.Empty);

						if (string.IsNullOrEmpty(input_inputFolder))
						{
							input_inputFolder = Path.Combine(Utility.Path.Project, InputPath);
						}
					}

					if (GUILayout.Button("刷新", GUILayout.Width(100)))
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
					GUILayout.Label("列表：", GUILayout.Width(100));

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
				GUILayout.Space(5);

				GUILayout.Box(string.Empty, GUILayout.ExpandWidth(true), GUILayout.Height(3));

				GUILayout.BeginHorizontal();
				{
					GUILayout.BeginVertical();
					{
						GUILayout.BeginHorizontal();
						{
							GUILayout.Label("※", GUILayout.Width(20));
							GUILayout.Label("名称", GUILayout.Width(120));
							GUILayout.Label("路径");
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

					GUILayout.Box(string.Empty, GUILayout.Width(3), GUILayout.ExpandHeight(true));

					GUILayout.Space(5);

					GUILayout.BeginVertical(GUILayout.Width(100));
					{
						if (GUILayout.Button("转换", GUILayout.Height(40)))
						{
							Convert();
						}

						if (GUILayout.Button("全选"))
						{
							Select(true);
						}

						if (GUILayout.Button("反选"))
						{
							Select(false);
						}

						if (GUILayout.Button("输入路径"))
						{
							OpenFolder(inputFolder);
						}

						if (GUILayout.Button("输出路径"))
						{
							OpenFolder(Path.Combine(Application.dataPath, outputFolder));
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
				GUILayout.Label(label_outputFolder, GUILayout.Width(100));

				if (GUILayout.Button(input_outputFolder))
				{
					input_outputFolder = EditorUtility.SaveFolderPanel("输出文件夹", Path.Combine(Application.dataPath, OutputPath), string.Empty);

					if (string.IsNullOrEmpty(input_outputFolder))
					{
						input_outputFolder = OutputPath;
					}
					else if (input_outputFolder.StartsWith(Application.dataPath))
					{
						input_outputFolder = input_outputFolder.Remove(0, Application.dataPath.Length + 1);
					}
					else
					{
						input_outputFolder = OutputPath;
					}
				}

				if (GUILayout.Button("保存", GUILayout.Width(100)))
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
								source.Add(new FileItem()
								{
									name = Path.GetFileNameWithoutExtension(path),
									path = Utility.Path.UnityToSystem(path),
									output = true,
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

						foreach (FileInfo file in root.GetFiles("*.psd", SearchOption.AllDirectories))
						{
							source.Add(new FileItem()
							{
								name = Path.GetFileNameWithoutExtension(file.Name),
								path = file.FullName,
								output = true,
							});
						}
					}
					break;
				default:
					if (Directory.Exists(Application.dataPath))
					{
						DirectoryInfo root = new DirectoryInfo(Application.dataPath);

						foreach (FileInfo file in root.GetFiles("*.psd", SearchOption.AllDirectories))
						{
							source.Add(new FileItem()
							{
								name = Path.GetFileNameWithoutExtension(file.Name),
								path = file.FullName,
								output = true,
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
				source[i].output = select;
			}
		}

		private void Convert()
		{
			string folder = node.Count > index_searchNode ? node[index_searchNode] : string.Empty;

			string path = string.Format("{0}/{1}/{2}", Application.dataPath, outputFolder, folder);

			for (int i = 0; i < source.Count; i++)
			{
				try
				{
					if (source[i].output)
					{
						Pfi.PfiImporter.AutoImport(source[i].path, Path.Combine(path, source[i].name));
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

			ShowNotification(new GUIContent("Convert Done!"));
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

	public class FileItem
	{
		public string name;

		public string path;

		public bool output;
	}
}