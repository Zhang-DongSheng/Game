using UnityEngine;

namespace UnityEditor.Window
{
	public partial class Artist : CustomWindow
	{
		private readonly string[] menu = new string[4] { "Main", "Asset", "Prefab", "Config" };
		[MenuItem("Art/Artist")]
		protected static void Open()
		{
			Open<Artist>("艺术家");
		}

		protected override void Init()
		{
			InitialisePrefab();
		}

		protected override void Refresh()
		{
			index.value = GUILayout.Toolbar(index.value, menu);

			GUILayout.BeginArea(new Rect(5, 30, Screen.width - 10, Screen.height - 50));
			{
				switch (index.value)
				{
					case 0:
						RefreshMd5();
						break;
					case 1:
						RefreshAsset();
						break;
					case 2:
						RefreshPrefab();
						break;
					default:
						RefreshConfig();
						break;
				}
			}
			GUILayout.EndArea();
		}
	}
}