using UnityEngine;

namespace Pfi
{
	public class PfiLayer
	{
		public string name;

		public PfiFolder parent;

		public Texture2D texture;

		public Vector2 position;

		public PfiLayer( string name )
		{
			this.name = name;
		}

		public override string ToString()
		{
			return this.name;
		}
	}
}