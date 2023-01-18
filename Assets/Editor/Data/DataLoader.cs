using Data;
using Game;
using System.Collections.Generic;
using UnityEditor.Window;
using UnityEngine;
using UnityEngine.U2D;
using Debuger = UnityEngine.Debuger;

namespace UnityEditor
{
    public class DataLoader : CustomWindow
    {
        #region 快捷方式
        [MenuItem("Data/Load/Text")]
        protected static void LoadDataText()
        {
            DataText data = Load<DataText>();

            string[] guids = AssetDatabase.FindAssets("t:TextAsset", new string[] { "Assets/Art/Language" });

            string path, file;

            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);

                file = System.IO.Path.GetFileNameWithoutExtension(path);

                if (GameConfig.Language.ToString().ToLower() == file)
                {
                    TextAsset asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

                    if (asset != null && !string.IsNullOrEmpty(asset.text))
                    {
                        data.dictionary = JsonUtility.FromJson<DataText.Dictionary>(asset.text);
                    }
                    break;
                }
            }
            EditorUtility.SetDirty(data);

            AssetDatabase.SaveAssets();
            
            AssetDatabase.Refresh();
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
                        path = path.Replace("Assets/", string.Empty),
                        sprites = new List<string>(asset.spriteCount),
                    };

                    Sprite[] sprites = new Sprite[asset.spriteCount];

                    asset.GetSprites(sprites);

                    for (int i = 0; i < sprites.Length; i++)
                    {
                        if (sprites[i] != null)
                        {
                            int length = sprites[i].name.Length;
                            // 删除结尾(Clone)
                            atlas.sprites.Add(sprites[i].name.Substring(0, length - 7));
                        }
                        else
                        {
                            Debuger.LogError(Author.Data, $"{asset.name}的第{i}张图片出问题了");
                        }
                    }
                    data.atlases.Add(atlas);
                }
            }
            EditorUtility.SetDirty(data);

            AssetDatabase.SaveAssets();

            AssetDatabase.Refresh();
        }
        #endregion
        [MenuItem("Data/Load/Editor", priority = 0)]
        protected static void Open()
        {
            Open<DataLoader>("数据加载");
        }

        protected override void Initialise()
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