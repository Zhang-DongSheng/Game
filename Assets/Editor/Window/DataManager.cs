﻿using Game;
using Game.Const;
using Game.Data;
using Game.UI;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Video;

namespace UnityEditor.Window
{
    public class DataManager : CustomWindow
    {
        private string path;

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
                    int count = _cells.Count;

                    for (int i = 0; i < count; i++)
                    {
                        RefreshLoading(_cells[i]);
                    }

                    if (GUILayout.Button(ToLanguage("Loading")))
                    {
                        for (int i = 0; i < count; i++)
                        {
                            if (_cells[i].asset && _cells[i].select)
                            {
                                Load(_cells[i].type);
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

                GUILayout.Label(cell.type.Name);

                if (cell.asset || cell.branch)
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

        private void RefreshSynchronizate()
        {
            if (GUILayout.Button(ToLanguage("Synchronizate") + " ProtoBuf"))
            {
                SynchronizateProtoBuf();
            }
        }

        private void RefreshOther()
        {
            if (GUILayout.Button(ToLanguage("Contact us")))
            {
                Utility.Common.OpenQQ(GameConfig.QQ);
            }
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
                    branch = type.Equals(typeof(DataLanguage)),
                    select = true,
                });
            }
        }

        #region Loading
        private void Create(Type type)
        {
            string path = $"{AssetPath.DataEditor}/{type.Name}.asset";
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
                Loading<DataConfig>("config");
            }
            else if (type == typeof(DataLanguage))
            {
                LoadLanguage();
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
            else
            {
                Debug.LogError("当前数据类型未定义，请主动添加 - " + type.Name);
            }
        }

        private void LoadPrefab()
        {
            var data = Load<DataUI>(); data.Clear();

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
            var data = Load<DataSprite>(); data.Clear();

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
                            Debug.LogError($"{asset.name} Atlas[{i}] is null!");
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
            var data = Load<DataAduio>(); data.Clear();

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
            var data = Load<DataVideo>(); data.Clear();

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
            string project = Application.dataPath.Substring(0, Application.dataPath.Length - 7);

            string input = string.Format("{0}/Source/Excel/language.xlsx", project);

            string output = string.Format("{0}/Assets/Art/Excel/language.json", project);

            ExcelUtility excel = new ExcelUtility(input);

            excel.ConvertToJson(output);

            string path = "Assets/Art/Excel/language.json";

            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            Debug.Assert(asset != null, string.Format("{0} is null", path));

            if (asset == null) return;

            string content = asset.text;

            if (string.IsNullOrEmpty(content)) return;

            foreach (var language in Enum.GetValues(typeof(Language)))
            {
                var data = Load<DataLanguage>(language.ToString());

                Debug.Assert(data != null, string.Format("Load DB_{0} error", typeof(T).Name));

                if (data == null) continue;

                data.language = (Language)language;

                data.Load(content);

                data.Sort();

                Save(data);
            }
        }

        private void Loading<T>(string source) where T : DataBase
        {
            string project = Application.dataPath.Substring(0, Application.dataPath.Length - 7);

            string input = string.Format("{0}/Source/Excel/{1}.xlsx", project, source);

            string output = string.Format("{0}/Assets/Art/Excel/{1}.json", project, source);

            ExcelUtility excel = new ExcelUtility(input);

            excel.ConvertToJson(output);

            string path = string.Format("Assets/Art/Excel/{0}.json", source);

            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            Debug.Assert(asset != null, string.Format("{0} is null", path));

            if (asset == null) return;

            string content = asset.text;

            if (string.IsNullOrEmpty(content)) return;

            T data = Load<T>();

            Debug.Assert(data != null, string.Format("Load DB_{0} error", typeof(T).Name));

            if (data == null) return;

            data.Load(content);

            data.Sort();

            Save(data);
        }

        private T Load<T>() where T : ScriptableObject
        {
            string path = $"{AssetPath.DataEditor}/{typeof(T).Name}.asset";

            T asset = AssetDatabase.LoadAssetAtPath<T>(path);

            return asset;
        }

        private T Load<T>(string branch) where T : ScriptableObject
        {
            string path = $"{AssetPath.DataEditor}/{typeof(T).Name}_{branch}.asset";

            T asset = AssetDatabase.LoadAssetAtPath<T>(path);

            return asset;
        }

        private bool Exist(Type type)
        {
            string path = $"{Application.dataPath}/{AssetPath.Data}/{type.Name}.asset";

            return File.Exists(path);
        }

        private void Save(UnityEngine.Object data)
        {
            EditorUtility.SetDirty(data);

            AssetDatabase.SaveAssets();

            AssetDatabase.Refresh();
        }

        private void Preview(Type type)
        {
            string path = $"{AssetPath.DataEditor}/{type.Name}.asset";

            var asset = AssetDatabase.LoadAssetAtPath(path, type);

            Selection.activeObject = asset;
        }
        #endregion

        #region Synchronizate
        private void SynchronizateILRuntime()
        {
            string key = "Hotfix";

            string src = string.Format("{0}/ILRuntime/Hotfix~/bin/Debug/{1}.dll", Application.dataPath, key);

            string dst = string.Format("{0}/ILRuntime/{1}.dll", Application.streamingAssetsPath, key);

            Replace(src, dst);

            src = string.Format("{0}/ILRuntime/Hotfix~/bin/Debug/{1}.pdb", Application.dataPath, key);

            dst = string.Format("{0}/ILRuntime/{1}.pdb", Application.streamingAssetsPath, key);

            Replace(src, dst);

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
                Debug.LogError("资源不存在！" + src);
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

            public bool branch;

            public bool select;
        }
    }
}