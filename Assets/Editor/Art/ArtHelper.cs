using System.IO;
using UnityEditor.SceneManagement;
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

		public static void CreateGameObject(string path)
		{
			Transform parent = Selection.activeTransform;

			GameObject clone, prefab = Resources.Load<GameObject>(path);

			if (prefab == null) return;

			if (prefab.TryGetComponent(out RectTransform _))
			{
				if (parent != null && parent.TryGetComponent(out RectTransform _))
				{
					clone = GameObject.Instantiate(prefab, parent);
				}
				else
				{
					Canvas canvas = FindObjectOfType<Canvas>();

					if (canvas == null)
					{
						canvas = new GameObject("Canvas").AddComponent<Canvas>();
					}
					parent = canvas.transform;

					clone = GameObject.Instantiate(prefab, parent);
				}
			}
			else
			{
				clone = GameObject.Instantiate(prefab, parent);
			}
			clone.name = clone.name.Replace("(Clone)", "").Trim();

			Undo.RegisterCreatedObjectUndo(clone, "Created an object");

			if (Application.isPlaying == false)
			{
				EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
			}
		}
	}
}