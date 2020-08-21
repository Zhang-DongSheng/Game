using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class AssetBundleTools : EditorWindow
{
	private readonly string[] text_view = new string[] { "PSD", "Setting", "Other" };

	private int index_view;

	[MenuItem("Data/AssetBundle")]
	private static void Open()
	{
		AssetBundleTools window = EditorWindow.GetWindow<AssetBundleTools>();
		window.titleContent = new GUIContent("AssetBundle Tools");
		window.minSize = new Vector2(500, 200);
		window.Show();
	}

	private void OnGUI()
	{
		index_view = GUILayout.Toolbar(index_view, text_view);

		GUILayout.BeginArea(new Rect(20, 30, Screen.width - 40, Screen.height - 50));
		{
			switch (index_view)
			{
				case 0:
					RefreshUIPSD();
					break;
				case 1:
					RefreshUISetting();
					break;
				default:
					RefreshUIOther();
					break;
			}
		}
		GUILayout.EndArea();
	}

	private void RefreshUIPSD()
	{
		if (GUILayout.Button("Convert"))
		{
			string folder = Application.dataPath.Remove(Application.dataPath.Length - 6) + "AssetBundles/Android1/";

			if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

			if (Selection.objects != null && Selection.objects.Length > 0)
			{
				List<AssetBundleBuild> builds = new List<AssetBundleBuild>();

				for (int i = 0; i < Selection.objects.Length; i++)
				{
					AssetBundleBuild build = new AssetBundleBuild()
					{
						assetBundleName = Selection.objects[i].name,
						assetNames = new string[] { AssetDatabase.GetAssetPath(Selection.objects[i]) }
					};
					builds.Add(build);
				}
				BuildPipeline.BuildAssetBundles(folder, builds.ToArray(), BuildAssetBundleOptions.None, BuildTarget.Android);
			}
		}
	}

	private void RefreshUISetting()
	{
		
	}

	private void RefreshUIOther()
	{
		if (GUILayout.Button("联系我们"))
		{
			Application.OpenURL("https://www.baidu.com");
		}
	}

	private void OpenFolder(string path)
	{
		if (string.IsNullOrEmpty(path)) return;

		if (Directory.Exists(path))
		{
			path = path.Replace("/", "\\");

			System.Diagnostics.Process.Start("explorer.exe", path);
		}
		else
		{
			Debug.LogError("No Directory: " + path);
		}
	}
}