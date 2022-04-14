using Data;
using System.Collections.Generic;
using UnityEditor.Window;
using UnityEngine;
using UnityEngine.U2D;

namespace UnityEditor
{
    public class DataLoader : CustomWindow
    {
        [MenuItem("Data/Load/Atlas")]
        protected static void LoadDataAtlas()
        {
            DataAtlas data = Load<DataAtlas>();

            data.atlases = new List<AtlasInformation>();

            string[] guids = AssetDatabase.FindAssets("t:SpriteAtlas", new string[] { "Assets" });

            string path;

            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);

                SpriteAtlas asset = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);

                if (asset != null)
                {
                    AtlasInformation atlas = new AtlasInformation()
                    {
                        name = asset.name,
                        path = path,
                        sprites = new List<string>(asset.spriteCount),
                    };

                    Sprite[] sprites = new Sprite[asset.spriteCount];

                    asset.GetSprites(sprites);

                    for (int i = 0; i < sprites.Length; i++)
                    {
                        atlas.sprites.Add(sprites[i].name.Remove(sprites[i].name.Length - 7, 7));
                    }
                    data.atlases.Add(atlas);
                }
            }
            AssetDatabase.SaveAssets(); AssetDatabase.Refresh();
        }

        protected override void Init()
        {
            throw new System.NotImplementedException();
        }

        protected override void Refresh()
        {
            throw new System.NotImplementedException();
        }

        public static T Load<T>() where T : ScriptableObject
        {
            string path = string.Format("Assets/Package/Data/{0}.asset", typeof(T).Name);

            T asset = AssetDatabase.LoadAssetAtPath<T>(path);

            return asset;
        }
    }
}