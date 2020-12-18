using System.Collections.Generic;
using System.IO;

namespace Utils
{
	public class Finder
	{
		public static List<Node> Find(string path)
		{
			List<Node> nodes = new List<Node>();

			Find(path, 0, ref nodes);

			return nodes;
		}

		private static void Find(string path, int order, ref List<Node> nodes)
		{
			if (Directory.Exists(path))
			{
				string[] dirs = Directory.GetDirectories(path);

				if (dirs.Length > 0)
				{
					foreach (string dir in dirs)
					{
						NodeFolder folder = new NodeFolder()
						{
							key = dir,
							path = dir,
							order = order,
							type = NodeType.Folder,
						};
						Find(dir, folder.order + 1, ref folder.nodes); nodes.Add(folder);
					}
				}

				DirectoryInfo root = new DirectoryInfo(path);

				foreach (FileInfo file in root.GetFiles())
				{
					nodes.Add(new NodeFile()
					{
						key = Path.GetFileNameWithoutExtension(file.Name),
						order = order,
						type = NodeType.File,
						path = file.FullName,
						extension = file.Extension,
					});
				}
			}
		}
	}

	public abstract class Node
	{
		public string key;

		public NodeType type;

		public int order;

		public string path;

		public string extension;
	}

	public class NodeFolder : Node
	{
		public bool status;

		public List<Node> nodes = new List<Node>();
	}

	public class NodeFile : Node
	{
		public bool select;
	}

	public enum NodeType
	{
		Folder,
		File,
	}
}