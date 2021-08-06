using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.Window
{
	public class Artist : CustomWindow
	{
		private readonly string[] menu = new string[4] { "Main", "Prefab", "Texture", "Other" };

		private readonly string[] assets = new string[5] { "None", "Sprite", "Texture", "Material", "TextAsset" };

		private readonly Index idxPrefab = new Index(), idxAsset = new Index();

		private readonly Index idxSrc = new Index(), idxDst = new Index();

		private string[] types;

		private List<ItemFile> list;

		private string[] _list;

		private string command = "Nothing";

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

			idxSrc.action = idxDst.action = (index) =>
			{
				if (idxSrc.index == 0 && idxDst.index == 0)
				{
					command = "Nothing";
				}
				else if (idxSrc.index == 0 && idxDst.index != 0)
				{
					command = "添加";
				}
				else if (idxSrc.index != 0 && idxDst.index != 0)
				{
					command = "替换";
				}
				else if (idxSrc.index != 0 && idxDst.index == 0)
				{
					command = "移除";
				}
			};

			types = TypeDefine.ToArrayString();
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
							ModifyGraphicColor(file.asset, color[0]);
						}
					});

					Horizontal(() =>
					{
						color[1] = EditorGUILayout.ColorField(color[1]);

						if (GUILayout.Button("文本"))
						{
							ModifyTextColor(file.asset, color[1]);
						}
					});

					Horizontal(() =>
					{
						color[2] = EditorGUILayout.ColorField(color[2]);

						if (GUILayout.Button("阴影"))
						{
							ModifyShadowColor(file.asset, color[2]);
						}
					});

					Horizontal(() =>
					{
						if (GUILayout.Button("触发:false"))
						{
							ModifyGraphicRaycast(file.asset);
						}
					});

					Horizontal(() =>
					{
						GUILayout.Label("Src");

						idxSrc.index = EditorGUILayout.Popup(idxSrc.index, types);
					});

					Horizontal(() =>
					{
						GUILayout.Label("Dst");

						idxDst.index = EditorGUILayout.Popup(idxDst.index, types);
					});

					if (GUILayout.Button(command))
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
			GUILayout.BeginHorizontal();
			{
				GUILayout.BeginVertical();
				{
					idxAsset.index = EditorGUILayout.Popup(idxAsset.index, assets);
				}
				GUILayout.EndVertical();

				GUILayout.Box(string.Empty, GUILayout.Width(3), GUILayout.ExpandHeight(true));

				GUILayout.BeginVertical(GUILayout.Width(200));
				{
					if (GUILayout.Button("检查资源引用"))
					{
						FindReferences.Empty(string.Format("t:{0}", assets[idxAsset.index]), "Assets");
					}
					if (GUILayout.Button("检查资源大小"))
					{
						FindReferences.Overflow(string.Format("t:{0}", assets[idxAsset.index]), "Assets");
					}
					if (GUILayout.Button("检查图片资源是否为4的倍数"))
					{
						FindReferences.Powof2("Assets");
					}
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
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

		private void ModifyGraphicColor(string path, Color color)
		{
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

			Graphic[] graphics = prefab.GetComponentsInChildren<Graphic>();

			for (int i = 0; i < graphics.Length; i++)
			{
				graphics[i].color = color;
			}
			PrefabUtility.SavePrefabAsset(prefab);
		}

		private void ModifyTextColor(string path, Color color)
		{
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

			Text[] texts = prefab.GetComponentsInChildren<Text>();

			for (int i = 0; i < texts.Length; i++)
			{
				texts[i].color = color;
			}
			PrefabUtility.SavePrefabAsset(prefab);
		}

		private void ModifyShadowColor(string path, Color color)
		{
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

			Shadow[] shadows = prefab.GetComponentsInChildren<Shadow>();

			for (int i = 0; i < shadows.Length; i++)
			{
				shadows[i].effectColor = color;
			}
			PrefabUtility.SavePrefabAsset(prefab);
		}

		private void ModifyGraphicRaycast(string path)
		{
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

			Graphic[] graphics = prefab.GetComponentsInChildren<Graphic>();

			for (int i = 0; i < graphics.Length; i++)
			{
				graphics[i].raycastTarget = false;
			}
			PrefabUtility.SavePrefabAsset(prefab);
		}

		private void ModifyComponent(string path, int src, int dst)
		{
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

			Type old = TypeDefine.GetType(src), now = TypeDefine.GetType(dst);

			if (old == null && now == null)
			{

			}
			else if (old == null && now != null)
			{
				if (prefab.TryGetComponent(now, out _)) { }
				else
				{
					prefab.AddComponent(now);
				}
			}
			else if (old != null && now != null)
			{
				Component[] components = prefab.GetComponentsInChildren(old);

				GameObject child;

				for (int i = 0; i < components.Length; i++)
				{
					child = components[i].gameObject;

					DestroyImmediate(components[i], true);

					if (child.TryGetComponent(now, out _)) { }
					else
					{
						child.AddComponent(now);
					}
				}
			}
			else if (old != null && now == null)
			{
				Component[] components = prefab.GetComponentsInChildren(old);

				for (int i = 0; i < components.Length; i++)
				{
					DestroyImmediate(components[i], true);
				}
			}
		}
	}
}