using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor.Window
{
	public class PrefabCopy : CustomWindow
	{
		private readonly float LINE = 30f;

		private readonly float MENU = 60f;

		private string source, target;

		private readonly List<Meta> list = new List<Meta>();

		[MenuItem("Assets/Copy Prefab", priority = 1)]
		protected static void Open()
		{
			if (Selection.activeGameObject != null)
			{
				Open<PrefabCopy>("预制体拷贝");
			}
			else
			{
				focusedWindow.ShowNotification(new GUIContent("深度拷贝需选中.Prefab文件！"));
			}
		}

		protected override void Init()
		{
			source = AssetDatabase.GetAssetPath(Selection.activeGameObject);

			input.value = Default;
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

				input.value = Default;
			}
		}
		#endregion

		#region UI
		protected override void Refresh()
		{
			GUILayout.BeginArea(new Rect(10, 10, Width - 20, Height - 20));
			{
				GUILayout.BeginVertical();
				{
					GUILayout.BeginHorizontal(GUILayout.Height(LINE));
					{
						GUILayout.Label("Asset:", GUILayout.Width(MENU));

						GUILayout.Label(source);
					}
					GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal(GUILayout.Height(LINE));
					{
						GUILayout.Label("Clone:", GUILayout.Width(MENU));

						if (GUILayout.Button(input.value))
						{
							input.value = EditorUtility.OpenFolderPanel("Clone", input.value, string.Empty);

							if (string.IsNullOrEmpty(input.value))
							{
								input.value = Default;
							}
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
			GUILayout.EndArea();
		}
		#endregion

		private void Copy()
		{
			if (!string.IsNullOrEmpty(source))
			{
				list.Clear();

				target = Format(source);

				string prefab = target;

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

		private string Format(string path)
		{
			string folder = input.value;

			string file = Path.GetFileNameWithoutExtension(path);

			string extension = Path.GetExtension(path);

			path = string.Format("{0}/{1}{2}", folder, file, extension);

			int index = 0;

			while (File.Exists(path))
			{
				path = string.Format("{0}/{1} Clone({2}){3}", folder, file, index++, extension);
			}
			return path;
		}

		private string Default
		{
			get
			{
				if (!string.IsNullOrEmpty(source))
				{
					return Path.GetDirectoryName(source).Replace('\\', '/') + "/Clone";
				}
				return string.Empty;

			}
		}

		class Meta
		{
			public string key;

			public string srcID;

			public string dstID;
		}
	}
}