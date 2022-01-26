using Game;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace UnityEditor.Window
{
    public class Artist : CustomWindow
	{
		private readonly string[] menu = new string[4] { "Main", "Asset", "Prefab", "Config" };

		private readonly string[] assets = new string[6] { "None", "TextAsset", "Texture", "Sprite", "Shader", "Material" };

		private readonly string[] shaders = new string[2] { "Reference", "Find" };

		private readonly Index indexAsset = new Index();

		private readonly Index indexPrefab = new Index();

		private readonly Index indexShader = new Index();

		private readonly Input inputShader = new Input();

		private readonly Input inputFile = new Input();

		private readonly Input inputString = new Input();

		private List<ItemFile> list;

		private string message;

		private string[] _list;

		private ItemFile file;

		private GameObject prefab;

		private readonly List<Object> dependencies = new List<Object>();

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

			indexPrefab.action = (index) =>
			{
				file = list[index];

				dependencies.Clear();

				if (string.IsNullOrEmpty(file.asset))
				{
					prefab = null;
				}
				else
				{
					prefab = AssetDatabase.LoadAssetAtPath<GameObject>(file.asset);

					string[] _dependencies = AssetDatabase.GetDependencies(file.asset);

					int count = _dependencies.Length;

					for (int i = 0; i < count; i++)
					{
						dependencies.Add(AssetDatabase.LoadAssetAtPath<Object>(_dependencies[i]));
					}
				}
			};
		}

		protected override void Refresh()
		{
			index.value = GUILayout.Toolbar(index.value, menu);

			GUILayout.BeginArea(new Rect(5, 30, Screen.width - 10, Screen.height - 50));
			{
				switch (index.value)
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
			GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
			{
				GUILayout.BeginHorizontal(GUILayout.Height(25));
				{
					GUILayout.Label("字符串", GUILayout.Width(50));

					inputString.value = GUILayout.TextField(inputString.value);

					if (GUILayout.Button("确定", GUILayout.Width(60)))
					{
						if (string.IsNullOrEmpty(inputString.value))
						{
							ShowNotification(new GUIContent("Error: Empty!"));
						}
						else
						{
							message = Utility.MD5.ComputeContent(inputString.value);
						}
					}
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal(GUILayout.Height(25));
				{
					GUILayout.Label("文件", GUILayout.Width(50));

					if (GUILayout.Button(inputFile.value))
					{
						inputFile.value = EditorUtility.OpenFilePanel("Md5", "", "");
					}
					if (GUILayout.Button("确定", GUILayout.Width(60)))
					{
						message = Utility.MD5.ComputeFile(inputFile.value);
					}
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal(GUILayout.Height(30));
				{
					GUILayout.Label("MD5", GUILayout.Width(50));

					GUILayout.Label(message, new GUIStyle() { alignment = TextAnchor.MiddleCenter, fontSize = 20 });

					if (GUILayout.Button("复制", GUILayout.Width(60)))
					{
						GUIUtility.systemCopyBuffer = message;
					}
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndArea();
		}

		private void RefreshAsset()
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.BeginVertical();
				{
					indexAsset.value = EditorGUILayout.Popup(indexAsset.value, assets);
				}
				GUILayout.EndVertical();

				GUILayout.Box(string.Empty, GUILayout.Width(3), GUILayout.ExpandHeight(true));

				GUILayout.BeginVertical(GUILayout.Width(200));
				{
					switch (indexAsset.value)
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
								indexShader.value = EditorGUILayout.Popup(indexShader.value, shaders);

								switch (indexShader.value)
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
											inputShader.value = GUILayout.TextField(inputShader.value);

											if (GUILayout.Button("查找引用该资源的所有Material"))
											{
												FindReferences.FindMaterialOfReferenceShader(inputShader.value);
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
									FindReferences.Empty(string.Format("t:{0}", assets[indexAsset.value]), "Assets");
								}
								if (GUILayout.Button("检查资源大小"))
								{
									FindReferences.Overflow(string.Format("t:{0}", assets[indexAsset.value]), "Assets");
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
				indexPrefab.value = EditorGUILayout.Popup(indexPrefab.value, _list);
			}
			GUILayout.EndHorizontal();

			GUILayout.Box(string.Empty, GUILayout.Height(3), GUILayout.ExpandWidth(true));

			if (file == null) return;

			GUILayout.BeginHorizontal();
			{
				GUILayout.BeginVertical();
				{
					EditorGUILayout.ObjectField(prefab, typeof(GameObject), false);

					int count = dependencies.Count;

					if (count > 0)
					{
						scroll = GUILayout.BeginScrollView(scroll);
						{
							GUI.enabled = false;

							for (int i = 0; i < count; i++)
							{
								RefreshPrefabDependence(dependencies[i]);
							}
							GUI.enabled = true;
						}
						GUILayout.EndScrollView();
					}
				}
				GUILayout.EndVertical();

				GUILayout.Box(string.Empty, GUILayout.Width(3), GUILayout.ExpandHeight(true));

				GUILayout.BeginVertical(GUILayout.Width(200));
				{
					if (GUILayout.Button("检查引用"))
					{
						if (indexPrefab.value == 0) { }
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
					if (GUILayout.Button("触发:false"))
					{
						PrefabModify.ModifyGraphicRaycast(file.asset);
					}
					if (GUILayout.Button("复制"))
					{
						Selection.activeObject = prefab;

						Open<PrefabCopy>("拷贝文件");
					}
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
		}

		private void RefreshPrefabDependence(Object meta)
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label(meta.name, GUILayout.Width(100));
				EditorGUILayout.ObjectField(meta, typeof(Object), false);
			}
			GUILayout.EndHorizontal();
		}

		private void RefreshOther()
		{
			
		}
	}
}