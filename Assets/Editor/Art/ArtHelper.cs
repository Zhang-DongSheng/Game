using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
	public class ArtHelper : EditorWindow
	{
		[MenuItem("Art/Helper")]
		protected static void Open()
		{
			ArtHelper window = EditorWindow.GetWindow<ArtHelper>();
			window.titleContent = new GUIContent("Art Helper");
			window.minSize = new Vector2(500, 200);
			window.Show();
		}

		private void OnGUI()
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

		private void Missing()
		{
			string[] files = Directory.GetFiles(Application.dataPath, "*.prefab", SearchOption.AllDirectories);

			foreach (var file in files)
			{
				GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(FindReferences.ToAssetPath(file));

				if (FindReferences.Missing(prefab))
				{
					Debug.LogError(prefab.name + " Has missing!");
				}
			}
		}
	}
}