using Game;
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

		private string extension;

		private readonly List<Meta> dependencies = new List<Meta>();

		[MenuItem("Assets/Copy Prefab", priority = 1)]
		protected static void Open()
		{
			if (Selection.activeObject != null)
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
			source = AssetDatabase.GetAssetPath(Selection.activeObject);

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
				source = AssetDatabase.GetAssetPath(Selection.activeObject);

				dependencies.Clear();

				string[] assets = AssetDatabase.GetDependencies(source);

				for (int i = 0; i < assets.Length; i++)
				{
					if (assets[i] == source) continue;

					extension = Path.GetExtension(assets[i]);

					if (string.IsNullOrEmpty(extension) || extension == ".cs")
					{
						continue;
					}
					//不做筛选
					if (true)
					{
						dependencies.Add(new Meta()
						{
							key = Path.GetFileNameWithoutExtension(assets[i]),
							path = assets[i],
							srcID = AssetDatabase.AssetPathToGUID(assets[i]),
							ignore = false,
						});
					}
				}
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
				target = FileUtils.New(source);

				string prefab = target;

				Copy(source, target);

				for (int i = 0; i < dependencies.Count; i++)
				{
					target = FileUtils.New(dependencies[i].path);

					if (dependencies[i].ignore)
					{
						dependencies[i].dstID = string.Empty;
					}
					else if (Copy(dependencies[i].path, target))
					{
						dependencies[i].dstID = AssetDatabase.AssetPathToGUID(target);
					}
				}
				AssetDatabase.Refresh();

				string content = File.ReadAllText(prefab);

				for (int i = 0; i < dependencies.Count; i++)
				{
					if (!string.IsNullOrEmpty(dependencies[i].dstID))
					{
						content = content.Replace(dependencies[i].srcID, dependencies[i].dstID);
					}
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
			public bool ignore;

			public string key;

			public string path;

			public string srcID;

			public string dstID;
		}
	}
}