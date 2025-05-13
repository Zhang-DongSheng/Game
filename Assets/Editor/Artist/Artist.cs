using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UnityEditor.Window
{
	public partial class Artist : CustomWindow
	{
		private readonly List<ArtistBase> artists = new List<ArtistBase>();

		private string[] options = new string[1] { "NONE" };
		[MenuItem("Game/Artist")]
		protected static void Open()
		{
			Open<Artist>("Artist");
		}

		protected override void Initialise()
		{
			List<Type> children = new List<Type>();

			Assembly assembly = Assembly.GetAssembly(typeof(ArtistBase));

			foreach (Type child in assembly.GetTypes())
			{
				if (child.BaseType == typeof(ArtistBase))
				{
					children.Add(child);
				}
			}
			artists.Clear();

			for (int i = 0; i < children.Count; i++)
			{
				if (Activator.CreateInstance(children[i]) is ArtistBase artist)
				{
					artists.Add(artist);
				}
				else
				{
					Debug.LogError("Initialise Fail！" + children[i].Name);
				}
			}
			int count = artists.Count;

			options = new string[count];

			for (int i = 0; i < count; i++)
			{
				options[i] = artists[i].Name;

				artists[i].Initialise();
			}
		}

		protected override void Refresh()
		{
			index.value = GUILayout.Toolbar(index.value, options);

			GUILayout.BeginArea(new Rect(5, 30, Screen.width - 10, Screen.height - 50));
			{
				if (artists.Count > index.value)
				{
					artists[index.value].Refresh();
				}
			}
			GUILayout.EndArea();
		}
	}
}