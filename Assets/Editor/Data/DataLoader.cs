using Data;
using Game;
using LitJson;
using System.Collections.Generic;
using UnityEditor.Window;
using UnityEngine;
using UnityEngine.U2D;
using Debuger = UnityEngine.Debuger;

namespace UnityEditor
{
    public class DataLoader : CustomWindow
    {
        protected static string PATH = "Assets/Art/Excel";
        #region ��ݷ�ʽ
        [MenuItem("Data/Load/Config")]
        protected static void LoadDataConfig()
        {
            DataText aa = new DataText();

            aa.dictionary = new DataText.Dictionary();

            aa.dictionary.words.Add(new DataText.Word()
            {
                key = "1",
                value = "2"
            });

            Debug.LogError(JsonMapper.ToJson(aa));
        }
        [MenuItem("Data/Load/Text")]
        protected static void LoadDataText()
        {
            string path = string.Format("{0}/{1}", PATH, "language.json");

            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            Debuger.Assert(asset != null, "����������Ϊ��");

            if (asset == null) return;

            string content = asset.text;

            if (string.IsNullOrEmpty(content)) return;

            DataText data = Load<DataText>();

            Debuger.Assert(data != null, "������DBΪ��");

            if (data == null) return;

            data.dictionary = new DataText.Dictionary();
            // һ��Ҫ�ǵ�ȥ�����һ�еĶ���
            JsonData json = JsonMapper.ToObject(content);

            if (json.ContainsKey("list"))
            {
                JsonData list = json.GetJson("list");

                string language = data.language.ToString().ToLower();

                int count = list.Count;

                for (int i = 0; i < count; i++)
                {
                    data.dictionary.words.Add(new DataText.Word()
                    {
                        key = list[i].GetString("key"),
                        value = list[i].GetString(language)
                    });
                }
            }
            else
            {
                Debuger.LogError(Author.Data, "�����Խ���ʧ��");
            }
            Save(data);
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
                            // ɾ����β(Clone)
                            atlas.sprites.Add(sprites[i].name.Substring(0, length - 7));
                        }
                        else
                        {
                            Debuger.LogError(Author.Data, $"{asset.name}�ĵ�{i}��ͼƬ��������");
                        }
                    }
                    data.atlases.Add(atlas);
                }
            }
            Save(data);
        }
        [MenuItem("Data/Load/Prop")]
        protected static void LoadDataProp()
        {
            string path = string.Format("{0}/{1}", PATH, "prop.json");

            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            Debuger.Assert(asset != null, "��������Ϊ��");

            if (asset == null) return;

            string content = asset.text;

            if (string.IsNullOrEmpty(content)) return;

            DataProp data = Load<DataProp>();

            Debuger.Assert(data != null, "����DBΪ��");

            if (data == null) return;

            data.props = new List<PropInformation>();
            // һ��Ҫ�ǵ�ȥ�����һ�еĶ���
            JsonData json = JsonMapper.ToObject(content);

            if (json.ContainsKey("list"))
            {
                JsonData list = json.GetJson("list");

                int count = list.Count;

                for (int i = 0; i < count; i++)
                {
                    data.props.Add(new PropInformation()
                    {
                        primary = list[i].GetUInt("ID"),
                        name = list[i].GetString("name"),
                        icon = list[i].GetString("icon"),
                        category = list[i].GetInt("category"),
                        quality = list[i].GetByte("quality"),
                        price = list[i].GetFloat("price"),
                        source = list[i].GetInt("source"),
                        description = list[i].GetString("description")
                    });
                }
            }
            else
            {
                Debuger.LogError(Author.Data, "�����Խ���ʧ��");
            }
            Save(data);
        }
        [MenuItem("Data/Load/All")]
        protected static void LoadAll()
        {

        }
        #endregion
        [MenuItem("Data/Load/Editor", priority = 0)]
        protected static void Open()
        {
            Open<DataLoader>("���ݼ���");
        }



        protected static T Load<T>() where T : ScriptableObject
        {
            string path = string.Format("Assets/Package/Data/{0}.asset", typeof(T).Name);

            T asset = AssetDatabase.LoadAssetAtPath<T>(path);

            return asset;
        }

        protected static void Save(Object data)
        {
            EditorUtility.SetDirty(data);

            AssetDatabase.SaveAssets();

            AssetDatabase.Refresh();
        }

        protected override void Initialise() { }

        protected override void Refresh() { }
    }
}