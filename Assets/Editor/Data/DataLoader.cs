using Data;
using Game.UI;
using System.Collections.Generic;
using UnityEditor.Window;
using UnityEngine;
using UnityEngine.U2D;
using Debuger = UnityEngine.Debuger;

namespace UnityEditor
{
    public class DataLoader : CustomWindow
    {
        class ItemInformation
        {
            public string name;

            public string type;

            public bool select;
        }
        private readonly List<ItemInformation> items = new List<ItemInformation>();

        protected const string PATH = "Assets/Art/Excel";
        [MenuItem("Data/Load", priority = 0)]
        protected static void Open()
        {
            Open<DataLoader>("数据加载");
        }

        protected override void Initialise()
        {
            items.Clear();

            items.Add(new ItemInformation()
            {
                name = "配置",
                type = typeof(DataConfig).ToString(),
                select = false,
            });
            items.Add(new ItemInformation()
            {
                name = "页面",
                type = typeof(DataUI).ToString(),
                select = false,
            });
            items.Add(new ItemInformation()
            {
                name = "语言",
                type = typeof(Language).ToString(),
                select = false,
            });
            items.Add(new ItemInformation()
            {
                name = "图集",
                type = typeof(DataSprite).ToString(),
                select = false,
            });
            items.Add(new ItemInformation()
            {
                name = "道具",
                type = typeof(DataProp).ToString(),
                select = false,
            });
            items.Add(new ItemInformation()
            {
                name = "商品",
                type = typeof(DataCommodity).ToString(),
                select = false,
            });
            items.Add(new ItemInformation()
            {
                name = "任务",
                type = typeof(DataTask).ToString(),
                select = false,
            });
        }

        protected override void Refresh()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(GUILayout.Width(Width - 200));
                {
                    scroll = GUILayout.BeginScrollView(scroll);
                    {
                        int count = items.Count;

                        for (int i = 0; i < count; i++)
                        {
                            RefreshItem(items[i]);
                        }
                    }
                    GUILayout.EndScrollView();
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                {
                    if (GUILayout.Button("Load"))
                    {
                        int count = items.Count;

                        for (int i = 0; i < count; i++)
                        {
                            if (items[i].select)
                            {
                                Load(items[i]);
                            }
                        }
                    }

                    if (GUILayout.Button("LoadAll"))
                    {
                        LoadAll();
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshItem(ItemInformation information)
        {
            GUILayout.BeginHorizontal();
            {
                information.select = EditorGUILayout.Toggle(information.select, GUILayout.Width(30));

                GUILayout.Space(10);

                GUILayout.Label(information.name);
            }
            GUILayout.EndHorizontal();
        }

        private void LoadDataConfig()
        {
            string path = string.Format("{0}/{1}", PATH, "config.json");

            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            Debuger.Assert(asset != null, string.Format("{0} is null", path));

            if (asset == null) return;

            string content = asset.text;

            if (string.IsNullOrEmpty(content)) return;

            DataConfig data = Load<DataConfig>();

            Debuger.Assert(data != null, "配置DB为空");

            if (data == null) return;

            data.Set(content);

            Save(data);
        }

        private void LoadDataUI()
        {
            DataUI data = Load<DataUI>();

            Debuger.Assert(data != null, "UI DB为空");

            if (data == null) return;

            data.list = new List<UIInformation>();

            foreach (var panel in System.Enum.GetValues(typeof(UIPanel)))
            {
                switch ((UIPanel)panel)
                {
                    case UIPanel.None:
                        break;
                    default:
                        {
                            string path = string.Format("{0}/{1}.prefab", UIConfig.Prefab, panel);

                            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/" + path);

                            if (prefab == null) continue;

                            if (prefab.TryGetComponent(out UIBase view))
                            {
                                data.list.Add(new UIInformation()
                                {
                                    panel = (UIPanel)panel,
                                    layer = view.layer,
                                    order = view.order,
                                    type = view.type,
                                    name = view.name,
                                    path = path,
                                    destroy = false
                                });
                            }
                        }
                        break;
                }
            }
            Save(data);
        }

        private void LoadDataText()
        {
            string path = string.Format("{0}/{1}", PATH, "language.json");

            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            Debuger.Assert(asset != null, string.Format("{0} is null", path));

            if (asset == null) return;

            string content = asset.text;

            if (string.IsNullOrEmpty(content)) return;

            //Dictionary data = Load<Dictionary>();

            //Debuger.Assert(data != null, "多语言DB为空");

            //if (data == null) return;

            //data.Set(content);

            //Save(data);
        }

        private void LoadDataSprite()
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
            Save(data);
        }

        private void LoadDataProp()
        {
            string path = string.Format("{0}/{1}", PATH, "prop.json");

            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            Debuger.Assert(asset != null, string.Format("{0} is null", path));

            if (asset == null) return;

            string content = asset.text;

            if (string.IsNullOrEmpty(content)) return;

            DataProp data = Load<DataProp>();

            Debuger.Assert(data != null, "道具DB为空");

            if (data == null) return;

            data.Set(content);

            Save(data);
        }

        private void LoadDataCommodity()
        {
            string path = string.Format("{0}/{1}", PATH, "commodity.json");

            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            Debuger.Assert(asset != null, string.Format("{0} is null", path));

            if (asset == null) return;

            string content = asset.text;

            if (string.IsNullOrEmpty(content)) return;

            DataCommodity data = Load<DataCommodity>();

            Debuger.Assert(data != null, "商品DB为空");

            if (data == null) return;

            data.Set(content);

            Save(data);
        }

        private void LoadDataTask()
        {
            string path = string.Format("{0}/{1}", PATH, "task.json");

            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            Debuger.Assert(asset != null, string.Format("{0} is null", path));

            if (asset == null) return;

            string content = asset.text;

            if (string.IsNullOrEmpty(content)) return;

            DataTask data = Load<DataTask>();

            Debuger.Assert(data != null, "任务DB为空");

            if (data == null) return;

            data.Set(content);

            Save(data);
        }

        private void Load(ItemInformation information)
        {
            if (information.type == typeof(DataConfig).ToString())
            {
                LoadDataConfig();
            }
            else if (information.type == typeof(DataUI).ToString())
            {
                LoadDataUI();
            }
            else if (information.type == typeof(Language).ToString())
            {
                LoadDataText();
            }
            else if (information.type == typeof(DataSprite).ToString())
            {
                LoadDataSprite();
            }
            else if (information.type == typeof(DataProp).ToString())
            {
                LoadDataProp();
            }
            else if (information.type == typeof(DataCommodity).ToString())
            {
                LoadDataCommodity();
            }
            else if (information.type == typeof(DataTask).ToString())
            {
                LoadDataTask();
            }
        }

        private void LoadAll()
        {
            int count = items.Count;

            for (int i = 0; i < count; i++)
            {
                Load(items[i]);
            }
        }

        private T Load<T>() where T : ScriptableObject
        {
            string path = string.Format("Assets/Package/Data/{0}.asset", typeof(T).Name);

            T asset = AssetDatabase.LoadAssetAtPath<T>(path);

            return asset;
        }

        private void Save(Object data)
        {
            EditorUtility.SetDirty(data);

            AssetDatabase.SaveAssets();

            AssetDatabase.Refresh();
        }
    }
}