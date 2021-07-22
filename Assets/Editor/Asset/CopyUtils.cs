using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor.Window
{
	public class CopyUtils : EditorWindow
	{
		private readonly float LINE = 30f;

		private readonly float MENU = 60f;

		private string _input, input, output = "copy";

		private string source, target;

		private int _index, index;

		private readonly List<Meta> list = new List<Meta>();

		private readonly List<string> children = new List<string>();

		[MenuItem("Assets/Copy", priority = 18)]
		protected static void Open()
		{
			if (Selection.activeGameObject != null)
			{
				CopyUtils window = GetWindow<CopyUtils>();
				window.titleContent = new GUIContent("��㿽��");
				window.minSize = new Vector2(600, 200);
				window.maxSize = new Vector2(900, 300);
				window.Init(); window.Show();
			}
			else
			{
				EditorUtility.DisplayDialog("��Դ����", "δѡ����ȷĿ��", "�ر�");
			}
		}

		private void Init()
		{
			source = AssetDatabase.GetAssetPath(Selection.activeGameObject);

			input = _input = "Assets/";

			index = _index = 0;

			Children(input);
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
					GUILayout.Label("[��Դ·��]", GUILayout.Width(MENU));

					GUILayout.TextField(source);

					if (GUILayout.Button("����", GUILayout.Width(100)))
					{
						Help();
					}
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal(GUILayout.Height(LINE));
				{
					GUILayout.Label("[���·��]", GUILayout.Width(MENU));

					_input = GUILayout.TextField(_input);

					if (!string.IsNullOrEmpty(_input) && _input.EndsWith("/"))
					{
						_input = _input.Substring(0, _input.Length - 1);
					}
					if (input != _input)
					{
						input = _input; Children(input);

						index = _index = 0;
					}
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal(GUILayout.Height(LINE));
				{
					GUILayout.Label("[���Ŀ��]", GUILayout.Width(MENU));

					output = GUILayout.TextField(output);

					if (children.Count > 0)
					{
						_index = EditorGUILayout.Popup(_index, children.ToArray(), GUILayout.Width(100));

						if (_index != 0)
						{
							if (index != _index)
							{
								index = _index;

								output = children[index];
							}
						}
					}
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
			if (path.StartsWith(input))
			{
				path = path.Remove(0, input.Length + 1);
			}

			int first = path.IndexOf("/");

			if (first > -1)
			{
				path = path.Remove(0, first);
			}
			path = string.Format("{0}/{1}/{2}", input, output, path);

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
			EditorUtility.DisplayDialog("����", @"
1.��Դ·�����ɱ༭�����ı�ѡ��Ŀ����Զ�����
2.���·��ΪĿ��ĸ�·��[Asset/...]
3.���Ŀ�����ļ��У��滻ѡ��Ŀ�꼰������ļ��ĵ�һ���ļ��С�
4.���ļ� = Asset/.../���Ŀ��/Ŀ���ļ�", "����");
		}
		#endregion
	}
}