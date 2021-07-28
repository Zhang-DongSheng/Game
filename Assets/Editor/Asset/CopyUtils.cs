using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor.Window
{
	public class CopyUtils : CustomWindow
	{
		private readonly float LINE = 30f;

		private readonly float MENU = 60f;

		private string output;

		private string source, target;

		private readonly List<Meta> list = new List<Meta>();

		private readonly List<string> children = new List<string>();

		[MenuItem("Assets/Copy", priority = 18)]
		protected static void Open()
		{
			if (Selection.activeGameObject != null)
			{
				Open<CopyUtils>("拷贝");
			}
			else
			{
				focusedWindow.ShowNotification(new GUIContent("深度拷贝需选中.Prefab文件！"));
			}
		}

		protected override void Init()
		{
			source = AssetDatabase.GetAssetPath(Selection.activeGameObject);

			index.index = 0;

			index.action = (index) =>
			{
				output = children[index];
			};

			input.value = Path.GetDirectoryName(source).Replace('\\', '/');

			input.action = (input) =>
			{
				Children(input); index.index = 0;
			};
			output = "copy";

			Children(input.value);
		}

		#region Event
		private void Awake()
		{
			Selection.selectionChanged += OnValueChanged;
		}

		private void OnDestroy()
		{
			Selection.selectionChanged -= OnValueChanged;
		}

		private void OnValueChanged()
		{
			if (Selection.activeGameObject != null)
			{
				source = AssetDatabase.GetAssetPath(Selection.activeGameObject);
			}
		}
		#endregion

		#region UI
		protected override void Refresh()
		{
			GUILayout.Space(15);

			GUILayout.BeginVertical();
			{
				GUILayout.BeginHorizontal(GUILayout.Height(LINE));
				{
					GUILayout.Label("[资源路径]", GUILayout.Width(MENU));

					GUILayout.TextField(source);

					if (GUILayout.Button("帮助", GUILayout.Width(100)))
					{
						Help();
					}
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal(GUILayout.Height(LINE));
				{
					GUILayout.Label("[输出路径]", GUILayout.Width(MENU));

					input.value = GUILayout.TextField(input.value);

					if (!string.IsNullOrEmpty(input.value) && input.value.EndsWith("/"))
					{
						input.value = input.value.Substring(0, input.value.Length - 1);
					}
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal(GUILayout.Height(LINE));
				{
					GUILayout.Label("[输出目标]", GUILayout.Width(MENU));

					output = GUILayout.TextField(output);

					if (children.Count > 0)
					{
						index.index = EditorGUILayout.Popup(index.index, children.ToArray(), GUILayout.Width(100));
					}
				}
				GUILayout.EndHorizontal();

				if (GUILayout.Button("Deep Copy", GUILayout.ExpandHeight(true)))
				{
					Copy();
				}
			}
			GUILayout.EndVertical();
		}
		#endregion

		#region Core
		protected void Copy()
		{
			if (!string.IsNullOrEmpty(source))
			{
				list.Clear();

				target = Format(source);

				string prefab = string.Format("{0}{1}", Application.dataPath, target.Remove(0, 6));

				Copy(source, target);

				string extension;

				string[] assets = AssetDatabase.GetDependencies(source);

				for (int i = 0; i < assets.Length; i++)
				{
					if (assets[i] == source) continue;

					extension = Path.GetExtension(assets[i]);

					if (string.IsNullOrEmpty(extension) ||
						extension == ".cs")
					{
						continue;
					}
					target = Format(assets[i]);

					if (Copy(assets[i], target))
					{
						list.Add(new Meta()
						{
							key = assets[i],
							srcID = AssetDatabase.AssetPathToGUID(assets[i]),
							dstID = AssetDatabase.AssetPathToGUID(target),
						});
					}
				}
				AssetDatabase.Refresh();

				string content = File.ReadAllText(prefab);

				for (int i = 0; i < list.Count; i++)
				{
					content = content.Replace(list[i].srcID, list[i].dstID);
				}
				File.WriteAllText(prefab, content);
			}
		}

		private bool Copy(string source, string target)
		{
			string folder = Path.GetDirectoryName(target);

			if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

			return AssetDatabase.CopyAsset(source, target);
		}

		class Meta
		{
			public string key;

			public string srcID;

			public string dstID;
		}
		#endregion

		#region Function
		private string Format(string path)
		{
			if (path.StartsWith(input.value))
			{
				path = path.Remove(0, input.value.Length + 1);
			}

			int first = path.IndexOf("/");

			if (first > -1)
			{
				path = path.Remove(0, first);
			}
			path = string.Format("{0}/{1}/{2}", input.value, output, path);

			string folder = Path.GetDirectoryName(path);

			string file = Path.GetFileNameWithoutExtension(path);

			string extension = Path.GetExtension(path);

			int index = 0;

			while (File.Exists(path))
			{
				path = string.Format("{0}/{1}_{2}{3}", folder, file, index++, extension);
			}
			return path;
		}

		private void Children(string path)
		{
			children.Clear();

			children.Add("Custom");

			path = string.Format("{0}{1}", Application.dataPath, path.Remove(0, 6));

			if (Directory.Exists(path))
			{
				foreach (var folder in Directory.GetDirectories(path))
				{
					children.Add(Path.GetFileNameWithoutExtension(folder));
				}
			}
		}
		#endregion

		#region Help
		private void Help()
		{
			EditorUtility.DisplayDialog("帮助", @"
1.资源路径不可编辑，当改变选中目标后自动更新
2.输出路径为目标的根路径[Asset/...]
3.输出目标是文件夹，替换选中目标及其关联文件的第一个文件夹。
4.新文件 = Asset/.../输出目标/目标文件", "已阅");
		}
		#endregion
	}
}