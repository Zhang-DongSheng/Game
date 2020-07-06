using System.Collections.Generic;

namespace Pfi
{
	public class PfiFolder
	{
		public string name;

		public PfiFolder parent;

		public List<PfiFolder> subFolders = new List<PfiFolder>();

		public List<PfiLayer> layers = new List<PfiLayer>();

		public PfiFolder( string name )
		{
			this.name = name;
		}

		public PfiFolder GetSubFolder( string name )
		{
			return this.subFolders.Find( f => f.name == name );
		}

		public override string ToString()
		{
			return this.name;
		}
	}
}