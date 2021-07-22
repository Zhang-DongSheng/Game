using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor.Window
{
	public class CopyUtils : EditorWindow
	{
		private readonly float LINE = 30f;

		private readonly float MENU = 60f;

		private string input, output = "copy";

		private string source, target;

		private readonly List<Meta> list = new List<Meta>();

		[MenuItem("Assets/Copy", priority = 18)]
		protected static void Open()
		{
			if (Selection.activeGameObject != null)
			{
				CopyUtils window = GetWindow<CopyUtils>();
				window.titleContent = new GUIContent("资源复制");
				window.minSize = new Vector2(500, 200);
				window.maxSize = new Vector2(500, 200);
				window.Init(); window.Show();
			}
			else
			{
				EditorUtility.DisplayDialog("资源复制", "未选中资源", "关闭");
			}
		}

		private void Init()
		{
			source = AssetDatabase.GetAssetPath(Selection.activeGameObject);

			input = "Assets/";
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
		private void OnGUI()
		{
			Refresh();
		}

		private void Refresh()
		{
			GUILayout.Space(15);

			GUILayout.BeginVertical();
			{
				GUILayout.BeginHorizontal(GUILayout.Height(LINE));
				{
					GUILayout.Label("[资源路径]", GUILayout.Width(MENU));

					GUILayout.TextField(source);
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal(GUILayout.Height(LINE));
				{
					GUILayout.Label("[资源路径]", GUILayout.Width(MENU));

					input = GUILayout.TextField(input);

					if (output.EndsWith("/"))
					{
						output = output.Substring(0, output.Length - 1);
					}
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal(GUILayout.Height(LINE));
				{
					GUILayout.Label("[文件夹名]", GUILayout.Width(MENU));

					output = GUILayout.TextField(output);
				}
				GUILayout.EndHorizontal();

				if (GUILayout.Button("Copy", GUILayout.ExpandHeight(true)))
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
			if (path.StartsWith(input))
			{
				path = path.Remove(0, input.Length + 1);
			}

			int index = path.IndexOf("/");

			if (index > -1)
			{
				path = path.Remove(0, index);
			}
			path = string.Format("{0}/{1}/{2}", input, output, path);

			return path;
		}
		#endregion
	}
}