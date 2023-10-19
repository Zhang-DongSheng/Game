using Codice.CM.SEIDInfo;
using Data;
using Game;
using Game.UI;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;

namespace UnityEditor.Window
{
    public class DataManager : CustomWindow
    {
        private const string source = "Assets/Art";

        private const string asset = "Assets/Package/Data";

        private readonly string[] _menu = new string[] { "配置", "加载", "同步", "其他" };

        private readonly List<DataCell> _cells = new List<DataCell>();

        [MenuItem("Game/Data")]
        protected static void Open()
        {
            Open<DataManager>("数据管理");
        }

        protected override void Initialise()
        {
            Reloading();
        }

        protected override void Refresh()
        {
            index.value = GUILayout.Toolbar(index.value, _menu);

            GUILayout.BeginArea(new Rect(20, 30, Screen.width - 40, Screen.height - 50));
            {
                switch (index.value)
                {
                    case 0:
                        RefreshConfig();
                        break;
                    case 1:
                        RefreshLoading();
                        break;
                    case 2:
                        RefreshSynchronizate();
                        break;
                    default:
                        RefreshOther();
                        break;
                }
            }
            GUILayout.EndArea();
        }

        private void RefreshConfig()
        {

        }

        private void RefreshLoading()
        {
            GUILayout.BeginVertical();
            {
                scroll = GUILayout.BeginScrollView(scroll);
                {
                    int count = _cells.Count;

                    for (int i = 0; i < count; i++)
                    {
                        RefreshLoading(_cells[i]);
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        private void RefreshLoading(DataCell cell)
        {
            GUILayout.BeginHorizontal();
            {
                cell.select = GUILayout.Toggle(cell.select, string.Empty);

                GUILayout.Label(cell.type.Name);

                if (cell.asset)
                {
                    if (GUILayout.Button("加载", GUILayout.Width(200)))
                    {
                        Load(cell.type);
                    }
                }
                else
                {
                    if (GUILayout.Button("创建", GUILayout.Width(200)))
                    {
                        Create(cell.type);
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshSynchronizate()
        {
            if (GUILayout.Button("同步ILRuntime"))
            {
                SynchronizateILRuntime();
            }

            if (GUILayout.Button("同步IFix"))
            {
                SynchronizateIFix();
            }
        }

        private void RefreshOther()
        {

        }

        private void Reloading()
        {
            _cells.Clear();

            var types = Extension.GetChildrenTypes(typeof(DataBase), false);

            foreach (var type in types)
            {
                _cells.Add(new DataCell()
                {
                    type = type,
                    asset = Exist(type),
                    select = false,
                });
            }
        }

        #region Loading
        private void Create(Type type)
        {
            string path = string.Format("{0}/{1}.asset", asset, type.Name);
            ScriptableObject script = ScriptableObject.CreateInstance(type);
            string folder = Path.GetDirectoryName(path);
            if (Directory.Exists(folder) == false)
                Directory.CreateDirectory(folder);
            AssetDatabase.CreateAsset(script, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(); Reloading();
        }

        private void Load(Type type)
        {
            if (type == typeof(DataConfig))
            {
                LoadConfig();
            }
            else if (type == typeof(DataLanguage))
            {
                LoadLanguage();
            }
            else if (type == typeof(DataSprite))
            {
                LoadSprite();
            }
            else if (type == typeof(DataUI))
            {
                LoadUI();
            }
            else if (type == typeof(DataProp))
            {
                LoadProp();
            }
            else if (type == typeof(DataCommodity))
            {
                LoadCommodity();
            }
            else if (type == typeof(DataTask))
            {
                LoadTask();
            }
            else
            {
                Debug.LogError("当前数据类型未定义，请主动添加 - " + type.Name);
            }
            Reloading();
        }

        private void LoadConfig()
        {
            string path = string.Format("{0}/{1}", source, "config.json");

            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            Debug.Assert(asset != null, string.Format("{0} is null", path));

            if (asset == null) return;

            string content = asset.text;

            if (string.IsNullOrEmpty(content)) return;

            DataConfig data = Load<DataConfig>();

            Debug.Assert(data != null, "����DBΪ��");

            if (data == null) return;

            data.Set(content);

            Save(data);
        }

        private void LoadUI()
        {
            DataUI data = Load<DataUI>();

            Debug.Assert(data != null, "UI DBΪ��");

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
                            string path = string.Format("{0}/{1}.prefab", UIDefine.Prefab, panel);

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

        private void LoadLanguage()
        {
            string path = string.Format("{0}/{1}", source, "language.json");

            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            Debug.Assert(asset != null, string.Format("{0} is null", path));

            if (asset == null) return;

            string content = asset.text;

            if (string.IsNullOrEmpty(content)) return;

            DataLanguage data = Load<DataLanguage>();

            Debug.Assert(data != null, "������DBΪ��");

            if (data == null) return;

            data.Set(content);

            Save(data);
        }

        private void LoadSprite()
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
                            Debug.LogError($"{asset.name}�ĵ�{i}��ͼƬ��������");
                        }
                    }
                    data.atlases.Add(atlas);
                }
            }
            Save(data);
        }

        private void LoadProp()
        {
            string path = string.Format("{0}/{1}", source, "prop.json");

            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            Debug.Assert(asset != null, string.Format("{0} is null", path));

            if (asset == null) return;

            string content = asset.text;

            if (string.IsNullOrEmpty(content)) return;

            DataProp data = Load<DataProp>();

            Debug.Assert(data != null, "����DBΪ��");

            if (data == null) return;

            data.Set(content);
            Save(data);
        }

        private void LoadCommodity()
        {
            string path = string.Format("{0}/{1}", source, "commodity.json");

            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            Debug.Assert(asset != null, string.Format("{0} is null", path));

            if (asset == null) return;

            string content = asset.text;

            if (string.IsNullOrEmpty(content)) return;

            DataCommodity data = Load<DataCommodity>();

            Debug.Assert(data != null, "��ƷDBΪ��");

            if (data == null) return;

            data.Set(content);
            Save(data);
        }

        private void LoadTask()
        {
            string path = string.Format("{0}/{1}", source, "task.json");

            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            Debug.Assert(asset != null, string.Format("{0} is null", path));

            if (asset == null) return;

            string content = asset.text;

            if (string.IsNullOrEmpty(content)) return;

            DataTask data = Load<DataTask>();

            Debug.Assert(data != null, "����DBΪ��");

            if (data == null) return;

            data.Set(content);

            Save(data);
        }

        private bool Exist(Type type)
        {
            string path = string.Format("{0}/Package/Data/{1}.asset", Application.dataPath, type.Name);

            return File.Exists(path);
        }

        private T Load<T>() where T : ScriptableObject
        {
            string path = string.Format("{0}/{1}.asset", DataManager.asset, typeof(T).Name);

            T asset = AssetDatabase.LoadAssetAtPath<T>(path);

            return asset;
        }

        private void Save(UnityEngine.Object data)
        {
            EditorUtility.SetDirty(data);

            AssetDatabase.SaveAssets();

            AssetDatabase.Refresh();
        }
        #endregion

        #region Synchronizate
        private void SynchronizateILRuntime()
        {
            string key = "Hotfix";

            string src = string.Format("{0}/ILRuntime/Hotfix~/bin/Debug/{1}.dll", Application.dataPath, key);

            string dst = string.Format("{0}/HotFix/{1}.dll", Application.streamingAssetsPath, key);

            Replace(src, dst);

            src = string.Format("{0}/ILRuntime/Hotfix~/bin/Debug/{1}.pdb", Application.dataPath, key);

            dst = string.Format("{0}/HotFix/{1}.pdb", Application.streamingAssetsPath, key);

            Replace(src, dst);

            ShowNotification("同步完成！");
        }

        private void SynchronizateIFix()
        {
            string key = "Assembly-CSharp.patch.bytes";

            string src = string.Format("{0}/{1}", Application.dataPath.Substring(0, Application.dataPath.Length - 7), key);

            if (File.Exists(src))
            {
                string dst = string.Format("{0}/Package/IFix/{1}", Application.dataPath, key);

                Replace(src, dst);

                ShowNotification("同步完成！");
            }
            else
            {
                Debug.LogError("文件不存在！" + src);
            }
        }

        private void Replace(string src, string dst)
        {
            if (File.Exists(dst))
            {
                File.Delete(dst);
            }
            File.Copy(src, dst);
        }
        #endregion

        class DataCell
        {
            public string name;

            public Type type;

            public bool asset;

            public bool select;
        }
    }
}
