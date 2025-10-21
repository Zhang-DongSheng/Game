using Game;
using Game.Data;
using Game.UI;
using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Utils;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Video;

namespace UnityEditor.Window
{
    public class DataManager : CustomWindow
    {
        private string path;

        private Vector2[] scrolls = new Vector2[2];

        private readonly string[] _menu = new string[] { "配置", "数据", "多语言", "同步", "其他" };

        private readonly Dictionary<string, string> _keys = new Dictionary<string, string>();

        private readonly List<DataCell> _datas = new List<DataCell>();

        private readonly List<DataCell> _languages = new List<DataCell>();
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
                        RefreshLanguage();
                        break;
                    case 3:
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
            bool lockFrame = QualitySettings.vSyncCount > 0;

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(ToLanguage("Frame"), GUILayout.Width(100));

                lockFrame = GUILayout.Toggle(lockFrame, string.Empty);

                QualitySettings.vSyncCount = lockFrame ? 1 : 0;
            }
            GUILayout.EndHorizontal();

            if (lockFrame)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(ToLanguage("Frame"), GUILayout.Width(100));

                    Application.targetFrameRate = EditorGUILayout.IntField(Application.targetFrameRate);
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(ToLanguage("Sleep"), GUILayout.Width(100));

                bool sleep = Screen.sleepTimeout == SleepTimeout.NeverSleep;

                sleep = GUILayout.Toggle(sleep, string.Empty);

                Screen.sleepTimeout = sleep ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(ToLanguage("Background"), GUILayout.Width(100));

                bool background = Application.runInBackground;

                background = GUILayout.Toggle(background, string.Empty);

                Application.runInBackground = background;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(ToLanguage("Time"), GUILayout.Width(100));

                Time.timeScale = EditorGUILayout.FloatField(Time.timeScale);
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshLoading()
        {
            GUILayout.BeginVertical();
            {
                scroll = GUILayout.BeginScrollView(scroll);
                {
                    int count = _datas.Count;

                    for (int i = 0; i < count; i++)
                    {
                        RefreshLoading(_datas[i]);
                    }

                    if (GUILayout.Button(ToLanguage("Loading")))
                    {
                        for (int i = 0; i < count; i++)
                        {
                            if (_datas[i].asset && _datas[i].select)
                            {
                                Load(_datas[i].type);
                            }
                        }
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
                cell.select = GUILayout.Toggle(cell.select, string.Empty, GUILayout.Width(50));

                GUILayout.Label(cell.name);

                if (cell.asset)
                {
                    if (GUILayout.Button(ToLanguage("Loading"), GUILayout.Width(100)))
                    {
                        Load(cell.type);
                    }

                    if (GUILayout.Button(ToLanguage("Preview"), GUILayout.Width(100)))
                    {
                        Preview(cell.type);
                    }
                }
                else
                {
                    if (GUILayout.Button(ToLanguage("Create"), GUILayout.Width(200)))
                    {
                        Create(cell.type);
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshLanguage()
        {
            GUILayout.BeginVertical();
            {
                scrolls[0] = GUILayout.BeginScrollView(scrolls[0]);
                {
                    int count = _languages.Count;

                    for (int i = 0; i < count; i++)
                    {
                        RefreshLanguage(_languages[i]);
                    }

                    if (GUILayout.Button(ToLanguage("Loading")))
                    {
                        LoadLanguage();
                    }
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label(ToLanguage("Find all language in project"));

                        if (GUILayout.Button(ToLanguage("Scan"), GUILayout.Width(100)))
                        {
                            ScanLanguage();
                        }
                    }
                    GUILayout.EndHorizontal();

                    foreach (var key in _keys)
                    {
                        RefreshLanguage(key.Key, key.Value);
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        private void RefreshLanguage(DataCell cell)
        {
            GUILayout.BeginHorizontal();
            {
                cell.select = GUILayout.Toggle(cell.select, string.Empty, GUILayout.Width(50));

                GUILayout.Label(cell.name);

                if (cell.asset)
                {
                    if (GUILayout.Button(ToLanguage("Preview"), GUILayout.Width(200)))
                    {
                        Preview(cell.type, cell.name);
                    }
                }
                else
                {
                    if (GUILayout.Button(ToLanguage("Create"), GUILayout.Width(200)))
                    {
                        Create(cell.type, cell.name);
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshLanguage(string key, string source)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(key, GUILayout.Width(250));

                GUILayout.Label(source, GUILayout.ExpandWidth(true));
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshSynchronizate()
        {
            if (GUILayout.Button(ToLanguage("Synchronizate") + " ProtoBuf"))
            {
                SynchronizateProtoBuf();
            }

            if (GUILayout.Button(ToLanguage("Synchronizate") + " HotUpdate"))
            {
                SynchronizateHotUpdate();
            }
        }

        private void RefreshOther()
        {
            if (GUILayout.Button(ToLanguage("Contact us")))
            {
                Utility.Common.OpenQQ(GameConfig.QQ);
            }
        }

        #region Loading
        private void Load(Type type)
        {
            if (type == typeof(DataConfig))
            {
                Loading<DataConfig>("config");
            }
            else if (type == typeof(DataSprite))
            {
                LoadSprite();
            }
            else if (type == typeof(DataAduio))
            {
                LoadAudio();
            }
            else if (type == typeof(DataVideo))
            {
                LoadVideo();
            }
            else if (type == typeof(DataUI))
            {
                LoadPrefab();
            }
            else if (type == typeof(DataProp))
            {
                Loading<DataProp>("prop");
            }
            else if (type == typeof(DataCommodity))
            {
                Loading<DataCommodity>("commodity");
            }
            else if (type == typeof(DataTask))
            {
                Loading<DataTask>("task");
            }
            else if (type == typeof(DataActivity))
            {
                Loading<DataActivity>("activity");
            }
            else if (type == typeof(DataRole))
            {
                Loading<DataRole>("role");
            }
            else if (type == typeof(DataDialog))
            {
                Loading<DataDialog>("dialog");
            }
            else if (type == typeof(DataAvatar))
            {
                Loading<DataAvatar>("avatar");
            }
            else
            {
                UnityEngine.Debuger.LogError(Author.Editor, "当前数据类型未定义，请主动添加 - " + type.Name);
            }
        }

        private void LoadPrefab()
        {
            var data = Load<DataUI>();

            data.Clear();

            foreach (var panel in Enum.GetValues(typeof(UIPanel)))
            {
                switch ((UIPanel)panel)
                {
                    case UIPanel.None:
                        break;
                    default:
                        {
                            path = string.Format("{0}/{1}View.prefab", AssetPath.UIPrefab, panel);

                            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/" + path);

                            if (prefab == null) continue;

                            if (prefab.TryGetComponent(out ViewBase view))
                            {
                                data.list.Add(new UIInformation()
                                {
                                    primary = (uint)Mathf.Abs(prefab.GetInstanceID()),
                                    name = view.name,
                                    panel = (int)panel,
                                    layer = view.layer,
                                    order = view.order,
                                    type = view.type,
                                    path = path,
                                    destroy = false
                                });
                            }
                        }
                        break;
                }
            }
            data.Detection();

            data.Sort();

            Save(data);
        }

        private void LoadSprite()
        {
            var data = Load<DataSprite>();

            data.Clear();

            string[] guids = AssetDatabase.FindAssets("t:SpriteAtlas", new string[] { "Assets/Package" });

            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);

                SpriteAtlas asset = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);

                if (asset != null)
                {
                    AtlasInformation atlas = new AtlasInformation()
                    {
                        primary = (uint)asset.GetInstanceID(),
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
                            // name whitout(Clone)
                            atlas.sprites.Add(sprites[i].name.Substring(0, length - 7));
                        }
                        else
                        {
                            UnityEngine.Debuger.LogError(Author.Editor, $"{asset.name} Atlas[{i}] is null!");
                        }
                    }
                    data.list.Add(atlas);
                }
            }
            data.Detection();

            data.Sort();

            Save(data);
        }

        private void LoadAudio()
        {
            var data = Load<DataAduio>();

            data.Clear();

            string[] guids = AssetDatabase.FindAssets("t:AudioClip", new string[] { "Assets/Package" });

            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);

                var asset = AssetDatabase.LoadAssetAtPath<AudioClip>(path);

                if (asset != null)
                {
                    data.list.Add(new AudioInformation()
                    {
                        primary = (uint)asset.GetInstanceID(),
                        name = asset.name,
                        loop = false,
                        time = asset.length,
                        path = path.Remove(0, 7),
                    });
                }
            }
            data.Detection();

            data.Sort();

            Save(data);
        }

        private void LoadVideo()
        {
            var data = Load<DataVideo>();

            data.Clear();

            string[] guids = AssetDatabase.FindAssets("t:VideoClip", new string[] { "Assets/Package" });

            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);

                var asset = AssetDatabase.LoadAssetAtPath<VideoClip>(path);

                if (asset != null)
                {
                    data.list.Add(new VideoInformation()
                    {
                        primary = (uint)asset.GetInstanceID(),
                        name = asset.name,
                        loop = false,
                        time = (float)asset.length,
                        path = path.Remove(0, 7),
                    });
                }
            }
            data.Detection();

            data.Sort();

            Save(data);
        }

        private void LoadLanguage()
        {
            string input = string.Format("{0}/Source/Excel/language.xlsx", AssetPath.Project);

            string output = string.Format("{0}/Art/Excel/language.json", Application.dataPath);

            bool exist = File.Exists(output);

            ExcelUtility excel = new ExcelUtility(input);

            excel.ConvertToJson(output);

            excel.Dispose();

            if (!exist)
            {
                AssetDatabase.ImportAsset(Utility.Path.SystemToUnity(output));
            }
            AssetDatabase.Refresh();

            var json = LoadJson<DataLanguage>();

            foreach (var language in Enum.GetValues(typeof(Language)))
            {
                var data = Load<DataLanguage>(language.ToString());

                UnityEngine.Debuger.Assert(data != null, $"Load language {language} error!");

                if (data == null) continue;

                data.language = (Language)language;

                data.Clear();

                data.Load(json);

                data.Sort();

                Save(data);
            }
        }

        private void Loading<T>(string source) where T : DataBase
        {
            string input = $"{AssetPath.Project}/Source/Excel/{source}.xlsx";

            string output = $"{Application.dataPath}/{AssetPath.Json}/{source}.json";

            bool exist = File.Exists(output);

            ExcelUtility excel = new ExcelUtility(input);

            excel.ConvertToJson(output);

            excel.Dispose();

            if (!exist)
            {
                AssetDatabase.ImportAsset(Utility.Path.SystemToUnity(output));
            }
            AssetDatabase.Refresh();

            T data = Load<T>();

            UnityEngine.Debuger.Assert(data != null, string.Format("Load DB_{0} error", typeof(T).Name));

            if (data == null) return;

            data.Clear();

            data.Load(LoadJson<T>());

            data.Sort();

            Save(data);
        }

        private T Load<T>(string branch = null) where T : ScriptableObject
        {
            if (string.IsNullOrEmpty(branch))
                path = $"Assets/{AssetPath.Data}/{typeof(T).Name}.asset";
            else
                path = $"Assets/{AssetPath.Data}/{typeof(T).Name}/{branch}.asset";
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            return asset;
        }

        private JsonData LoadJson<T>()
        {
            var name = typeof(T).Name.ToLower()[4..];

            string path = $"Assets/{AssetPath.Json}/{name}.json";

            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            UnityEngine.Debuger.Assert(asset != null, string.Format("{0} is null", path));

            if (asset == null) return null;

            string content = asset.text;

            var json = JsonMapper.ToObject(content);

            if (json.ContainsKey("list"))
            {
                return json["list"];
            }
            return json;
        }

        private void Create(Type type, string branch = null)
        {
            if (string.IsNullOrEmpty(branch))
                path = $"Assets/{AssetPath.Data}/{type.Name}.asset";
            else
                path = $"Assets/{AssetPath.Data}/{type.Name}/{branch}.asset";
            ScriptableObject script = ScriptableObject.CreateInstance(type);
            string folder = Path.GetDirectoryName(path);
            if (Directory.Exists(folder) == false)
                Directory.CreateDirectory(folder);
            AssetDatabase.CreateAsset(script, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(); Reloading();
        }

        private bool Exist(string name)
        {
            string path = $"{Application.dataPath}/{AssetPath.Data}/{name}.asset";

            return File.Exists(path);
        }

        private void Save(UnityEngine.Object data)
        {
            EditorUtility.SetDirty(data);

            AssetDatabase.SaveAssets();

            AssetDatabase.Refresh();
        }

        private void Preview(Type type, string branch = null)
        {
            if (string.IsNullOrEmpty(branch))
                path = $"Assets/{AssetPath.Data}/{type.Name}.asset";
            else
                path = $"Assets/{AssetPath.Data}/{type.Name}/{branch}.asset";
            var asset = AssetDatabase.LoadAssetAtPath(path, type);

            Selection.activeObject = asset;
        }
        #endregion

        private void Reloading()
        {
            _datas.Clear(); _languages.Clear();

            var types = Extension.GetChildrenTypes(typeof(DataBase), false);

            foreach (var type in types)
            {
                if (type == typeof(DataLanguage)) continue;

                _datas.Add(new DataCell()
                {
                    type = type,
                    name = type.Name,
                    asset = Exist(type.Name),
                    select = true,
                });
            }
            // Language
            foreach (var language in Enum.GetValues(typeof(Language)))
            {
                _languages.Add(new DataCell()
                {
                    type = typeof(DataLanguage),
                    name = language.ToString(),
                    asset = Exist($"DataLanguage/{language}"),
                    select = true,
                });
            }
        }

        private void ScanLanguage()
        {
            _keys.Clear();

            _keys.AddRange(AssetUtils.ScanKeysInPrefab());

            _keys.AddRange(AssetUtils.ScanKeysInText());

            _keys.AddRange(AssetUtils.ScanKeysInTable());

            var asset = Load<DataLanguage>(Language.Chinese.ToString());

            if (asset == null) return;

            int count = asset.list.Count;

            for (int i = 0; i < count; i++)
            {
                if (_keys.ContainsKey(asset.list[i].key))
                {
                    _keys.Remove(asset.list[i].key);
                }
            }
        }

        #region Synchronizate
        private void SynchronizateHotUpdate()
        {
            var platform = EditorUserBuildSettings.activeBuildTarget.ToString();

            var dll = "Assembly-HotUpdate.dll";

            var src = $"{AssetPath.Project}/HybridCLRData/HotUpdateDlls/{platform}/{dll}";

            var dst = $"{Application.streamingAssetsPath}/{dll}.bytes";

            Utility.Document.Copy(src, dst);

            AssetDatabase.Refresh();

            ShowNotification("同步完成！");
        }

        private void SynchronizateProtoBuf()
        {
            string root = Application.dataPath;

            string key = "Utils/ProtoBuf/protoc-3.17.3-win64/bin/output";

            string src = string.Format("{0}/{1}", root.Substring(0, root.Length - 7), key);

            if (Directory.Exists(src))
            {
                var files = new Dictionary<string, int>();

                int index = 1;

                foreach (var file in Directory.GetFiles(src))
                {
                    var name = Path.GetFileNameWithoutExtension(file);

                    if (name.StartsWith("C2S") || name.StartsWith("S2C"))
                    {
                        if (files.ContainsKey(name))
                        {

                        }
                        else
                        {
                            files.Add(name, index++);
                        }
                    }
                }
                ScriptUtils.ModifyNetworkMessageDefine(files);

                string dst = string.Format("{0}/Scripts/Data/Proto", root);

                if (Directory.Exists(dst))
                {
                    Directory.Delete(dst, true);
                }
                Utility.Document.Copy(src, dst);

                AssetDatabase.Refresh();

                ShowNotification("同步完成！");
            }
            else
            {
                UnityEngine.Debuger.LogError(Author.Editor, "资源不存在！" + src);
            }
        }
        #endregion

        class DataCell
        {
            public Type type;

            public string name;

            public bool asset;

            public bool select;
        }
    }
}