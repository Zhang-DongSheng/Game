using Data;
using System.Collections.Generic;
using UnityEditor.Window;
using UnityEngine;
using UnityEngine.U2D;

namespace UnityEditor
{
    public class DataLoader : CustomWindow
    {
        #region 快捷方式
        [MenuItem("Data/Load/Text")]
        protected static void LoadDataText()
        {
            DataText data = Load<DataText>();

            data.words.Clear();

            AssetDatabase.SaveAssets(); AssetDatabase.Refresh();
        }
        [MenuItem("Data/Load/Sprite")]
        protected static void LoadDataSprite()
        {
            DataSprite data = Load<DataSprite>();

            data.atlases = new List<AtlasInformation>();

            string[] guids = AssetDatabase.FindAssets("t:SpriteAtlas", new string[] { "Assets/Package" });

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
                        path = path.Replace("Assets/Package/", string.Empty),
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
        #endregion
        [MenuItem("Data/Load/Editor", priority = 0)]
        protected static void Open()
        {
            Open<DataLoader>("数据加载");
        }

        protected override void Init()
        {
            
        }

        protected override void Refresh()
        {
            
        }

        public static T Load<T>() where T : ScriptableObject
        {
            string path = string.Format("Assets/Package/Data/{0}.asset", typeof(T).Name);

            T asset = AssetDatabase.LoadAssetAtPath<T>(path);

            return asset;
        }
    }
}