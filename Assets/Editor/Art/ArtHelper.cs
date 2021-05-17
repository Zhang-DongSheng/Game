using System.Collections;
using System.Collections.Generic;
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

		}
	}
}