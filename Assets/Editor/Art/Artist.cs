using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.Window
{
	public class Artist : CustomWindow
	{
		private readonly string[] menu = new string[4] { "Main", "Prefab", "Texture", "Other" };

		private readonly Index idxPrefab = new Index();

		private readonly Index idxSrc = new Index(), idxDst = new Index();

		private List<ItemFile> list;

		private string[] _list;

		private ItemFile file;

		private GameObject prefab;

		private readonly Color[] color = new Color[3] { Color.white, Color.white, Color.white };

		[MenuItem("Art/Artist")]
		protected static void Open()
		{
			Open<Artist>("艺术家");
		}

		protected override void Init()
		{
			list = Finder.LoadFiles(Application.dataPath, "*.prefab");

			_list = new string[list.Count];

			for (int i = 0; i < list.Count; i++)
			{
				_list[i] = list[i].name;
			}

			idxPrefab.action = (index) =>
			{
				file = list[index];

				if (string.IsNullOrEmpty(file.asset))
				{
					prefab = null;
				}
				else
				{
					prefab = AssetDatabase.LoadAssetAtPath<GameObject>(file.asset);
				}
			};
		}

		protected override void Refresh()
		{
			index.index = GUILayout.Toolbar(index.index, menu);

			GUILayout.BeginArea(new Rect(5, 30, Screen.width - 10, Screen.height - 50));
			{
				switch (index.index)
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

		}

		private void RefreshPrefab()
		{
			GUILayout.BeginHorizontal();
			{
				idxPrefab.index = EditorGUILayout.Popup(idxPrefab.index, _list);
			}
			GUILayout.EndHorizontal();

			GUILayout.Box(string.Empty, GUILayout.Height(3), GUILayout.ExpandWidth(true));

			if (file == null) return;

			GUILayout.BeginHorizontal();
			{
				GUILayout.BeginVertical();
				{
					EditorGUILayout.ObjectField(prefab, typeof(GameObject), false);
				}
				GUILayout.EndVertical();

				GUILayout.Box(string.Empty, GUILayout.Width(3), GUILayout.ExpandHeight(true));

				GUILayout.BeginVertical(GUILayout.Width(200));
				{
					if (GUILayout.Button("检查"))
					{
						if (idxPrefab.index == 0)
						{

						}
						else
						{
							Missing(file.asset);
						}
					}

					Horizontal(() =>
					{
						color[0] = EditorGUILayout.ColorField(color[0]);

						if (GUILayout.Button("图像"))
						{
							ModifyGraphic(file.asset, color[0]);
						}
					});

					Horizontal(() =>
					{
						color[1] = EditorGUILayout.ColorField(color[1]);

						if (GUILayout.Button("文本"))
						{
							ModifyText(file.asset, color[1]);
						}
					});

					Horizontal(() =>
					{
						color[2] = EditorGUILayout.ColorField(color[2]);

						if (GUILayout.Button("阴影"))
						{
							ModifyShadow(file.asset, color[2]);
						}
					});

					Horizontal(() =>
					{
						GUILayout.Label("Src");

						idxSrc.index = EditorGUILayout.Popup(idxSrc.index, menu);
					});

					Horizontal(() =>
					{
						GUILayout.Label("Dst");

						idxDst.index = EditorGUILayout.Popup(idxDst.index, menu);
					});

					if (GUILayout.Button("替换"))
					{
						ModifyComponent(file.asset, idxSrc.index, idxDst.index);
					}
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
		}

		private void RefreshTexture()
		{
			if (GUILayout.Button("检查引用次数为空的图片"))
			{
				FindReferences.Empty("t:Sprite", "Assets/Resources");
			}

			if (GUILayout.Button("检查图片大小"))
			{

			}
		}

		private void RefreshOther()
		{

		}

		private void Missing(params string[] files)
		{
			foreach (var file in files)
			{
				GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(file);

				if (FindReferences.Missing(prefab)) { }
				else
				{
					Debug.LogFormat("<color=green>[{0}]</color> 无丢失引用", prefab.name);
				}
			}
		}

		private void ModifyGraphic(string path, Color color)
		{
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

			Graphic[] graphics = prefab.GetComponentsInChildren<Graphic>();

			for (int i = 0; i < graphics.Length; i++)
			{
				graphics[i].color = color;
			}
			PrefabUtility.SavePrefabAsset(prefab);
		}

		private void ModifyText(string path, Color color)
		{
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

			Text[] texts = prefab.GetComponentsInChildren<Text>();

			for (int i = 0; i < texts.Length; i++)
			{
				texts[i].color = color;
			}
			PrefabUtility.SavePrefabAsset(prefab);
		}

		private void ModifyShadow(string path, Color color)
		{
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

			Shadow[] shadows = prefab.GetComponentsInChildren<Shadow>();

			for (int i = 0; i < shadows.Length; i++)
			{
				shadows[i].effectColor = color;
			}
			PrefabUtility.SavePrefabAsset(prefab);
		}

		private void ModifyComponent(string path, int src, int dst)
		{

		}
	}
}