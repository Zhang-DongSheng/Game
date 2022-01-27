using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor
{
    public class PrefabModify
	{
		public static void Missing(params string[] files)
		{
			foreach (var file in files)
			{
				GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(file);

				if (AssetDetection.Missing(prefab)) { }
				else
				{
					Debug.LogFormat("<color=green>[{0}]</color> ÎÞ¶ªÊ§ÒýÓÃ", prefab.name);
				}
			}
		}

		public static void ModifyGraphicColor(string path, Color color)
		{
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

			Graphic[] graphics = prefab.GetComponentsInChildren<Graphic>();

			for (int i = 0; i < graphics.Length; i++)
			{
				graphics[i].color = color;
			}
			PrefabUtility.SavePrefabAsset(prefab);
		}

		public static void ModifyTextColor(string path, Color color)
		{
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

			Text[] texts = prefab.GetComponentsInChildren<Text>();

			for (int i = 0; i < texts.Length; i++)
			{
				texts[i].color = color;
			}
			PrefabUtility.SavePrefabAsset(prefab);
		}

		public static void ModifyShadowColor(string path, Color color)
		{
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

			Shadow[] shadows = prefab.GetComponentsInChildren<Shadow>();

			for (int i = 0; i < shadows.Length; i++)
			{
				shadows[i].effectColor = color;
			}
			PrefabUtility.SavePrefabAsset(prefab);
		}

		public static void ModifyGraphicRaycast(string path)
		{
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

			Graphic[] graphics = prefab.GetComponentsInChildren<Graphic>();

			for (int i = 0; i < graphics.Length; i++)
			{
				graphics[i].raycastTarget = false;
			}
			PrefabUtility.SavePrefabAsset(prefab);
		}
	}
}