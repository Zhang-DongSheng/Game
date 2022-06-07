using Game;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;

namespace UnityEditor.Window
{
	public class PrefabCopy : CustomWindow
	{
		private readonly float LINE = 30f;

		private readonly float MENU = 60f;

		private string source, target;

		private string extension;

		private bool selectLock = false;

		private readonly List<Meta> dependencies = new List<Meta>();

		[MenuItem("Assets/CopyPrefab", priority = 0)]
		protected static void Open()
		{
			Open<PrefabCopy>("预制体拷贝");
		}
		[MenuItem("Assets/CopyPrefab", true, priority = 0)]
		protected static bool Detection()
		{
			return Selection.activeObject != null && AssetDetection(Selection.activeObject);
		}

		private static bool AssetDetection(Object asset)
		{
			if (asset != null)
			{
				if (asset is GameObject ||
					asset is Material ||
					asset is ScriptableObject ||
					asset is SpriteAtlas)
				{
					return true;
				}
			}
			return false;
		}

		protected override void Init()
		{
			OnValueChanged();
		}

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
			if (Selection.activeObject != null && AssetDetection(Selection.activeObject) && !selectLock)
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
					dependencies.Add(new Meta()
					{
						key = Path.GetFileNameWithoutExtension(assets[i]),
						path = assets[i],
						src = AssetDatabase.AssetPathToGUID(assets[i]),
						asset = AssetDatabase.LoadAssetAtPath<Object>(assets[i]),
						ignore = false,
					});
				}
				input.value = Default;
			}
		}

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

						selectLock = GUILayout.Toggle(selectLock, "锁定", GUILayout.Width(50));
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

					GUILayout.BeginHorizontal();
					{
						GUILayout.BeginVertical();
						{
							int count = dependencies.Count;

							if (count > 0)
							{
								GUILayout.BeginHorizontal();
								{
									GUILayout.Label("名称", GUILayout.ExpandWidth(true));
									GUILayout.Label("资源", GUILayout.Width(200));
									GUILayout.Label("忽略", GUILayout.Width(30));
								}
								GUILayout.EndHorizontal();

								scroll = GUILayout.BeginScrollView(scroll);
								{
									for (int i = 0; i < count; i++)
									{
										RefreshItem(dependencies[i]);
									}
								}
								GUILayout.EndScrollView();
							}
						}
						GUILayout.EndVertical();

						GUILayout.Box(string.Empty, GUILayout.ExpandHeight(true), GUILayout.Width(3));

						if (GUILayout.Button("复制", GUILayout.ExpandHeight(true), GUILayout.Width(150)))
						{
							Copy();
						}
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		}

		private void RefreshItem(Meta meta)
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label(meta.key, GUILayout.ExpandWidth(true));
				GUI.enabled = false;
				EditorGUILayout.ObjectField(meta.asset, typeof(Object), false, GUILayout.Width(200));
				GUI.enabled = true;
				meta.ignore = GUILayout.Toggle(meta.ignore, string.Empty, GUILayout.Width(30));
			}
			GUILayout.EndHorizontal();
		}

		private void Copy()
		{
			if (!string.IsNullOrEmpty(source))
			{
				target = Utility._Path.New(string.Format("{0}/{1}", input.value, Path.GetFileName(source)));

				string prefab = target;

				Copy(source, target);

				for (int i = 0; i < dependencies.Count; i++)
				{
					target = Utility._Path.New(dependencies[i].path);

					if (dependencies[i].ignore)
					{
						dependencies[i].dst = string.Empty;
					}
					else if (Copy(dependencies[i].path, target))
					{
						dependencies[i].dst = AssetDatabase.AssetPathToGUID(target);
					}
				}
				AssetDatabase.Refresh();

				string content = File.ReadAllText(prefab);

				for (int i = 0; i < dependencies.Count; i++)
				{
					if (!string.IsNullOrEmpty(dependencies[i].dst))
					{
						content = content.Replace(dependencies[i].src, dependencies[i].dst);
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
					return Path.GetDirectoryName(source).Replace('\\', '/');
				}
				return string.Empty;
			}
		}

		class Meta
		{
			public bool ignore;

			public string key;

			public string path;

			public string src;

			public string dst;

			public Object asset;
		}
	}
}