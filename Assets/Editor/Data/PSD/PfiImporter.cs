using Data;
using SubjectNerd.PsdImporter.PsdParser;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pfi
{
	public static class PfiImporter
	{
		public static void Menu_ImportPsd()
		{
			var file = Selection.activeObject;
			var filePath = AssetDatabase.GetAssetPath(file);
			using (var psd = PsdDocument.Create(filePath))
			{
				Import(filePath);
			}
		}

		public static PfiDocument Import(string psdPath)
		{
			using (var psd = PsdDocument.Create(psdPath))
			{
				var doc = ProcessDocument(psd);
				doc.name = Path.GetFileNameWithoutExtension(psdPath);
				return doc;
			}
		}

		private static PfiDocument ProcessDocument(PsdDocument psd)
		{
			var doc = new PfiDocument();
			doc.size = new Vector2(psd.Width, psd.Height);
			doc.root = new PfiFolder("");
			ProcessChildren(psd, doc.root, psd);
			return doc;
		}

		private static void ProcessChildren(PsdDocument psd, PfiFolder folder, IPsdLayer psdLayer)
		{
			for (int i = 0; i < psdLayer.Childs.Length; i++)
			{
				var child = psdLayer.Childs[i] as PsdLayer;
				if (child.HasImage)
				{
					var layer = ProcessImageLayer(psd, child);
					layer.parent = folder;
					folder.layers.Add(layer);
				}
				else
				{
					var childFolder = new PfiFolder(child.Name);
					childFolder.parent = folder;
					ProcessChildren(psd, childFolder, child);
					folder.subFolders.Add(childFolder);
				}
			}
		}

		private static PfiLayer ProcessImageLayer(PsdDocument psd, PsdLayer psdLayer)
		{
			var layer = new PfiLayer(psdLayer.Name);
			layer.texture = GetLayerTexture(psdLayer);

			var x = psdLayer.Left + psdLayer.Width / 2;
			var y = psd.Height - (psdLayer.Top + psdLayer.Height / 2);
			x -= psd.Width / 2;
			y -= psd.Height / 2;
			layer.position = new Vector2(x, y);

			return layer;
		}

		private static void SaveTexture(Texture2D texture, string path)
		{
			var data = texture.EncodeToPNG();
			File.WriteAllBytes(path, data);
			AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
		}

		private static object GetLayerFullName(PsdLayer layer)
		{
			var name = layer.Name;
			var current = layer.Parent;
			while (current != null)
			{
				name = current.Name + "_" + name;
				current = current.Parent;
			}
			return name;
		}

		private static Texture2D GetLayerTexture(PsdLayer layer)
		{
			Texture2D texture = new Texture2D(layer.Width, layer.Height);
			Color32[] pixels = new Color32[layer.Width * layer.Height];

			Channel red = (from l in layer.Channels where l.Type == ChannelType.Red select l).First();
			Channel green = (from l in layer.Channels where l.Type == ChannelType.Green select l).First();
			Channel blue = (from l in layer.Channels where l.Type == ChannelType.Blue select l).First();
			Channel alpha = (from l in layer.Channels where l.Type == ChannelType.Alpha select l).FirstOrDefault();
			Channel mask = (from l in layer.Channels where l.Type == ChannelType.Mask select l).FirstOrDefault();

			for (int i = 0; i < pixels.Length; i++)
			{
				byte r = red.Data[i];
				byte g = green.Data[i];
				byte b = blue.Data[i];
				byte a = 255;

				if (alpha != null)
					a = alpha.Data[i];
				if (mask != null)
					a *= mask.Data[i];

				int mod = i % texture.width;
				int n = ((texture.width - mod - 1) + i) - mod;
				pixels[pixels.Length - n - 1] = new Color32(r, g, b, a);
			}

			texture.SetPixels32(pixels);
			texture.Apply();
			return texture;
		}

		public static void AutoImport(string input, string output)
		{
			PfiDocument doc = Import(input);

			if (!Directory.Exists(output)) Directory.CreateDirectory(output);

			if (doc != null && doc.root.layers.Count > 0)
			{
				PfiLayer layer;

				for (int i = 0; i < doc.root.layers.Count; i++)
				{
					layer = doc.root.layers[i];

					if (layer != null)
					{
						string path = string.Format("{0}/{1}.png", output, layer.name);
						byte[] buffer = layer.texture.EncodeToPNG();
						if (File.Exists(path))
							File.Delete(path);
						File.WriteAllBytes(path, buffer);
					}
				}
			}

			ImportAsset(output, doc);
		}

		private static void ImportAsset(string path, PfiDocument doc, string assetName = "psd")
		{
			path = path.Replace(Application.dataPath, "Assets");

			DataPSD asset = AssetDatabase.LoadAssetAtPath<DataPSD>(string.Format("{0}/{1}.asset", path, assetName));

			if (asset == null)
			{
				asset = ScriptableObject.CreateInstance<DataPSD>();
				AssetDatabase.CreateAsset(asset, string.Format("{0}/{1}.asset", path, assetName));
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}

			PSDInformation psd = new PSDInformation()
			{
				name = path.Remove(0, path.LastIndexOf('/') + 1),
			};

			if (doc != null && doc.root.layers.Count > 0)
			{
				PfiLayer layer;

				for (int i = 0; i < doc.root.layers.Count; i++)
				{
					layer = doc.root.layers[i];

					if (layer != null)
					{
						psd.sprites.Add(new SpriteInformation()
						{
							name = layer.name,
							sprite = AssetDatabase.LoadAssetAtPath<Texture2D>(string.Format("{0}/{1}.png", path, layer.name)),
							position = layer.position,
							size = new Vector2(layer.texture.width, layer.texture.height),
							order = i,
						});
					}
				}
			}
			asset.list.Add(psd);

			AssetDatabase.SaveAssets();
		}
	}
}