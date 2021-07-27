using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor.Window
{
	public class ArtHelper : CustomWindow
	{
		private readonly string[] menu = new string[4] { "Main", "Prefab", "Texture", "Other" };

		private int indexView, _indexView;

		private int indexPrefab, _indexPrefab;

		private string[] _list;

		private List<ItemFile> list;

		[MenuItem("Art/Helper")]
		protected static void Open()
		{
			Open<ArtHelper>("“’ ıº“");
		}

		protected override void Init()
		{
			list = Finder.LoadFiles(Application.dataPath, "*.prefab");

			_list = new string[list.Count];

			for (int i = 0; i < list.Count; i++)
			{
				_list[i] = list[i].name;
			}
		}

		protected override void Refresh()
		{
			_indexView = GUILayout.Toolbar(_indexView, menu);

			if (indexView != _indexView)
			{
				indexView = _indexView;
			}

			GUILayout.BeginArea(new Rect(20, 30, Screen.width - 40, Screen.height - 50));
			{
				switch (indexView)
				{
					case 0:
						RefreshMain();
						break;
					case 1:
						RefreshPrefab();
						break;
					case 2:
						RefreshTexture();
						break;
					default:
						RefreshOther();
						break;
				}
			}
			GUILayout.EndArea();
		}

		private void RefreshMain()
		{
			if (GUILayout.Button("Empty Sprite"))
			{
				FindReferences.Empty("t:Sprite", "Assets/Resources");
			}

			if (GUILayout.Button("Miss"))
			{
				Missing();
			}
		}

		private void RefreshPrefab()
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.BeginVertical();
				{

				}
				GUILayout.EndVertical();

				GUILayout.Box(string.Empty, GUILayout.Width(3), GUILayout.ExpandHeight(true));

				GUILayout.BeginHorizontal(GUILayout.Width(120));
				{
					_indexPrefab = EditorGUILayout.Popup(_indexPrefab, _list);

					if (indexPrefab != _indexPrefab)
					{
						indexPrefab = _indexPrefab;
					}
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndHorizontal();
		}

		private void RefreshTexture()
		{

		}

		private void RefreshOther()
		{

		}

		private void Missing()
		{
			string[] files = Directory.GetFiles(Application.dataPath, "*.prefab", SearchOption.AllDirectories);

			foreach (var file in files)
			{
				GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(ToAssetPath(file));

				if (FindReferences.Missing(prefab))
				{
					Debug.LogError(prefab.name + " Has missing!");
				}
			}
		}
	}
}