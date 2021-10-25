using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.Window
{
	public class Artist : CustomWindow
	{
		private readonly string[] menu = new string[4] { "Main", "Asset", "Prefab", "Config" };

		private readonly string[] assets = new string[6] { "None", "TextAsset", "Texture", "Sprite", "Shader", "Material" };

		private readonly string[] shaders = new string[2] { "Reference", "Find" };

		private readonly Index idxPrefab = new Index(), idxAsset = new Index();

		private readonly Index idxShader = new Index();

		private readonly Input iptShader = new Input();

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
						RefreshAsset();
						break;
					case 2:
						RefreshPrefab();
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

		private void RefreshAsset()
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
					switch (idxAsset.index)
					{
						case 0:
							{
								GUILayout.Label(string.Empty);
							}
							break;
						case 2:
						case 3:
							{
								if (GUILayout.Button("检查图片资源是否为4的倍数"))
								{
									FindReferences.Powerof2("Assets");
								}
								goto default;
							}
						case 4:
							{
								idxShader.index = EditorGUILayout.Popup(idxShader.index, shaders);

								switch (idxShader.index)
								{
									case 0:
										{
											if (GUILayout.Button("查找所有未被引用的Shader"))
											{
												FindReferences.FindUnreferencedShader();
											}
											if (GUILayout.Button("查找所有被引用的Shader"))
											{
												FindReferences.FindReferenceShader();
											}
										}
										break;
									case 1:
										{
											iptShader.value = GUILayout.TextField(iptShader.value);

											if (GUILayout.Button("查找引用该资源的所有Material"))
											{
												FindReferences.FindMaterialOfReferenceShader(iptShader.value);
											}
										}
										break;
								}
							}
							break;
						default:
							{
								if (GUILayout.Button("检查资源引用"))
								{
									FindReferences.Empty(string.Format("t:{0}", assets[idxAsset.index]), "Assets");
								}
								if (GUILayout.Button("检查资源大小"))
								{
									FindReferences.Overflow(string.Format("t:{0}", assets[idxAsset.index]), "Assets");
								}
							}
							break;
					}
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
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
					if (GUILayout.Button("检查引用"))
					{
						if (idxPrefab.index == 0) { }
						else
						{
							PrefabModify.Missing(file.asset);
						}
					}

					Horizontal(() =>
					{
						color[0] = EditorGUILayout.ColorField(color[0]);

						if (GUILayout.Button("图像"))
						{
							PrefabModify.ModifyGraphicColor(file.asset, color[0]);
						}
					});

					Horizontal(() =>
					{
						color[1] = EditorGUILayout.ColorField(color[1]);

						if (GUILayout.Button("文本"))
						{
							PrefabModify.ModifyTextColor(file.asset, color[1]);
						}
					});

					Horizontal(() =>
					{
						color[2] = EditorGUILayout.ColorField(color[2]);

						if (GUILayout.Button("阴影"))
						{
							PrefabModify.ModifyShadowColor(file.asset, color[2]);
						}
					});

					Horizontal(() =>
					{
						if (GUILayout.Button("触发:false"))
						{
							PrefabModify.ModifyGraphicRaycast(file.asset);
						}
					});
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
		}

		private void RefreshOther()
		{

		}
	}
}