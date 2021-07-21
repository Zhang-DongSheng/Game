using System.Collections.Generic;
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
		[MenuItem("Assets/Copy")]
		protected static void Copy()
		{
			GameObject select = Selection.activeGameObject;

			if (select != null)
			{
				List<MetaInformation> metas = new List<MetaInformation>();

				string source = AssetDatabase.GetAssetPath(select);

				string[] assets = AssetDatabase.GetDependencies(source);

				string path = Path.GetDirectoryName(source) + "/copy";

				path = path.Replace('\\', '/');

				if (!Directory.Exists(path)) Directory.CreateDirectory(path);

				string target = string.Format("{0}/{1}", path, Path.GetFileName(source));

				string extension;

				Copy(source, target);

				for (int i = 0; i < assets.Length; i++)
				{
					extension = Path.GetExtension(assets[i]);

					if (string.IsNullOrEmpty(extension) ||
						extension == ".cs")
					{
						continue;
					}
					target = string.Format("{0}/{1}", path, Path.GetFileName(assets[i]));

					if (Copy(assets[i], target))
					{
						metas.Add(new MetaInformation()
						{
							key = assets[i],
							source = AssetDatabase.AssetPathToGUID(assets[i]),
							target = AssetDatabase.AssetPathToGUID(target),
						});
					}
				}
				AssetDatabase.Refresh();

				string prefab = string.Format("{0}{1}/{2}", Application.dataPath, path.Remove(0, 6), Path.GetFileName(source));

				string content = File.ReadAllText(prefab);

				for (int i = 0; i < metas.Count; i++)
				{
					content = content.Replace(metas[i].source, metas[i].target);
				}
				File.WriteAllText(prefab, content);
			}
		}

		protected static bool Copy(string source, string target)
		{
			string folder = Path.GetDirectoryName(target);

			if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

			return AssetDatabase.CopyAsset(source, target);
		}
	}

	public class MetaInformation
	{
		public string key;

		public string source;

		public string target;
	}
}