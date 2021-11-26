using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IngameDebugConsole
{
	public class DebugLogHelper
	{
		const int ScriptExecuteOrder = -12000;

		const string Prefab = "IngameDebugConsole";

		[MenuItem("Component/Loger")]
		public static void CreateReporter()
		{
			DebugLogManager loger = GameObject.FindObjectOfType<DebugLogManager>();

			if (loger != null)
			{
				EditorWindow.focusedWindow.ShowNotification(new GUIContent("Loger“—¥Ê‘⁄"));
				return;
			}

			string[] guids = AssetDatabase.FindAssets(string.Format("t:prefab {0}", Prefab));

			if (guids.Length == 0)
			{
				EditorWindow.focusedWindow.ShowNotification(new GUIContent(string.Format("Can't find the prefab : {0}", Prefab)));
				return;
			}

			Transform parent = CreateRoot();

			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guids[0]));

			GameObject view = GameObject.Instantiate(prefab, parent);

			Canvas canvas = view.GetComponent<Canvas>();

			canvas.overridePixelPerfect = false;

			canvas.overrideSorting = true;

			canvas.sortingOrder = 1000;

			view.name = Prefab;

			MonoScript script = MonoScript.FromMonoBehaviour(view.GetComponent<DebugLogManager>());

			if (MonoImporter.GetExecutionOrder(script) != ScriptExecuteOrder)
			{
				MonoImporter.SetExecutionOrder(script, ScriptExecuteOrder);
			}
		}

		protected static Transform CreateRoot()
		{
			Canvas canvas = GameObject.FindObjectOfType<Canvas>();

			if (canvas == null)
			{
				GameObject go = new GameObject("Canvas");
				canvas = go.AddComponent<Canvas>();
				go.AddComponent<CanvasScaler>();
				go.AddComponent<GraphicRaycaster>();

				GameObject system = new GameObject("EventSystem");
				system.AddComponent<EventSystem>();
				system.AddComponent<StandaloneInputModule>();
			}
			return canvas.transform;
		}
	}
}