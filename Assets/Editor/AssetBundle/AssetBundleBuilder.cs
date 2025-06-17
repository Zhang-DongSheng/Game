using Game;
using Game.Resource;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor.Window
{
    class AssetBundleBuilder : CustomWindow
    {
        private readonly List<string> IgnoreExtensionList = new List<string>() { ".meta", ".manifest" };

        #region GUI
        private readonly string[] text_view = new string[] { "资源", "Bundle", "其他" };

        private int index_view;

        private int index_source;

        private int index_assetbundle;

        private string input_source;

        private string input_assetbundle;
        #endregion

        private int currentViewIndex = -1;

        private int currentSourceIndex = -1;

        private int currentAssetbundleIndex = -1;

        private string[] text_folder_source = new string[] { "None" };

        private string[] text_folder_assetbundle = new string[] { "None" };

        private readonly List<ItemFolder> folder_source = new List<ItemFolder>();

        private readonly List<ItemFolder> folder_assetbundle = new List<ItemFolder>();

        private readonly List<ItemFile> items = new List<ItemFile>();

        [MenuItem("AssetBundle/Editor")]
        protected static void Open()
        {
            Open<AssetBundleBuilder>("AssetBundle工具");
        }

        protected override void Initialise()
        {
            UpdateAssetFolder();

            UpdateAssetBundleFolder();
        }

        private void UpdateAssetFolder()
        {
            string path = SourcePath;

            folder_source.Clear();

            if (Directory.Exists(path))
            {
                DirectoryInfo root = new DirectoryInfo(path);

                text_folder_source = new string[1 + root.GetDirectories().Length];

                int index = 0;

                folder_source.Add(new ItemFolder()
                {
                    name = root.Name,
                    path = root.FullName,
                });
                text_folder_source[index++] = root.Name;

                foreach (DirectoryInfo folder in root.GetDirectories())
                {
                    folder_source.Add(new ItemFolder()
                    {
                        name = string.Format("{0}/{1}", root.Name, folder.Name),
                        path = folder.FullName,
                    });
                    text_folder_source[index++] = folder.Name;
                }
            }
            else
            {
                Directory.CreateDirectory(path);
            }
        }

        private void UpdateAssetBundleFolder()
        {
            string path = AssetBundlePath + "/" + Game.Const.AssetPath.Package;

            folder_assetbundle.Clear();

            if (Directory.Exists(path))
            {
                DirectoryInfo root = new DirectoryInfo(path);

                text_folder_assetbundle = new string[1 + root.GetDirectories().Length];

                int index = 0;

                folder_assetbundle.Add(new ItemFolder()
                {
                    name = root.Name,
                    path = root.FullName,
                });
                text_folder_assetbundle[index++] = root.Name;

                foreach (DirectoryInfo folder in root.GetDirectories())
                {
                    folder_assetbundle.Add(new ItemFolder()
                    {
                        name = string.Format("{0}/{1}", root.Name, folder.Name),
                        path = folder.FullName,
                    });
                    text_folder_assetbundle[index++] = folder.Name;
                }
            }
            else
            {
                Directory.CreateDirectory(path);
            }
        }

        protected override void Refresh()
        {
            index_view = GUILayout.Toolbar(index_view, text_view);

            if (currentViewIndex != index_view)
            {
                currentSourceIndex = currentAssetbundleIndex = -1;

                currentViewIndex = index_view;
            }

            GUILayout.BeginArea(new Rect(20, 30, Screen.width - 40, Screen.height - 50));
            {
                switch (currentViewIndex)
                {
                    case 0:
                        RefreshUIAsset();
                        break;
                    case 1:
                        RefreshUIAssetBundle();
                        break;
                    default:
                        RefreshUIOther();
                        break;
                }
            }
            GUILayout.EndArea();
        }

        private void RefreshUIAsset()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Asset:", GUILayout.Width(50));

                index_source = EditorGUILayout.Popup(index_source, text_folder_source);

                if (currentSourceIndex != index_source)
                {
                    currentSourceIndex = index_source;

                    if (folder_source.Count > currentSourceIndex)
                    {
                        if (currentSourceIndex == 0)
                            LoadAll(folder_source[0].path, folder_source[0].name, input_source, true);
                        else
                            Load(folder_source[currentSourceIndex].path, folder_source[currentSourceIndex].name, true);
                    }
                    else
                    {
                        items.Clear();
                    }
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                if (currentSourceIndex == 0)
                {
                    GUILayout.Label(ToLanguage("Search"), GUILayout.Width(50));

                    input_source = GUILayout.TextField(input_source);

                    if (GUILayout.Button(ToLanguage("Refresh"), GUILayout.Width(100)))
                    {
                        if (folder_source.Count > 0)
                        {
                            LoadAll(folder_source[0].path, folder_source[0].name, input_source);
                        }
                    }
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            {
                if (items.Count > 0)
                {
                    RefreshUIList();

                    GUILayout.Space(5);

                    GUILayout.BeginVertical(GUILayout.Width(100));
                    {
                        if (GUILayout.Button(ToLanguage("Build AssetBundle"), GUILayout.Height(30)))
                        {
                            BuildAssetBundle();
                        }

                        if (GUILayout.Button(ToLanguage("Select All")))
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                items[i].select = true;
                            }
                        }

                        if (GUILayout.Button(ToLanguage("Cancel")))
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                items[i].select = false;
                            }
                        }

                        if (GUILayout.Button(ToLanguage("Assets Folder")))
                        {
                            OpenFolder(SourcePath);
                        }

                        if (GUILayout.Button(ToLanguage("AssetsBundle Folder")))
                        {
                            OpenFolder(AssetBundlePath);
                        }
                    }
                    GUILayout.EndVertical();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshUIAssetBundle()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Asset:", GUILayout.Width(50));

                index_assetbundle = EditorGUILayout.Popup(index_assetbundle, text_folder_assetbundle);

                if (currentAssetbundleIndex != index_assetbundle)
                {
                    currentAssetbundleIndex = index_assetbundle;

                    if (folder_assetbundle.Count > currentAssetbundleIndex)
                    {
                        if (currentAssetbundleIndex == 0)
                            LoadAll(folder_assetbundle[0].path, folder_assetbundle[0].name, input_assetbundle);
                        else
                            Load(folder_assetbundle[currentAssetbundleIndex].path, folder_assetbundle[currentAssetbundleIndex].name);
                    }
                    else
                    {
                        items.Clear();
                    }
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                if (currentAssetbundleIndex == 0)
                {
                    GUILayout.Label("Search:", GUILayout.Width(50));

                    input_assetbundle = GUILayout.TextField(input_assetbundle);

                    if (GUILayout.Button(ToLanguage("Refresh"), GUILayout.Width(100)))
                    {
                        if (folder_assetbundle.Count > 0)
                        {
                            LoadAll(folder_assetbundle[0].path, folder_assetbundle[0].name, input_assetbundle);
                        }
                    }
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            {
                if (items.Count > 0)
                {
                    RefreshUIList();

                    GUILayout.Space(5);

                    GUILayout.BeginVertical(GUILayout.Width(100));
                    {
                        if (GUILayout.Button(ToLanguage("Upload"), GUILayout.Height(60)))
                        {
                            Record(HistoryPath, string.Empty, items);
                            Upload();
                        }

                        if (GUILayout.Button(ToLanguage("Select All")))
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                items[i].select = true;
                            }
                        }

                        if (GUILayout.Button(ToLanguage("Cancel")))
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                items[i].select = false;
                            }
                        }

                        if (GUILayout.Button(ToLanguage("Preview") + "md5"))
                        {
                            OpenFileMd5();
                        }

                        if (GUILayout.Button(ToLanguage("Update") + "md5"))
                        {
                            UploadMd5();
                        }
                    }
                    GUILayout.EndVertical();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshUIList()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(ToLanguage("S"), GUILayout.Width(20));
                    GUILayout.Label(ToLanguage("Name"), GUILayout.Width(120));
                    GUILayout.Label(ToLanguage("Path"));
                    GUILayout.Label(ToLanguage("Size"), GUILayout.Width(100));
                }
                GUILayout.EndHorizontal();

                scroll = GUILayout.BeginScrollView(scroll);
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            items[i].select = GUILayout.Toggle(items[i].select, string.Empty, GUILayout.Width(20));
                            GUILayout.Label(items[i].name, GUILayout.Width(120));
                            GUILayout.Label(items[i].path);
                            GUILayout.Label(items[i].size.ToStringSize(), GUILayout.Width(100));
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        private void RefreshUIOther()
        {
            if (GUILayout.Button(ToLanguage("Contact us")))
            {
                Utility.Common.OpenQQ(GameConfig.QQ);
            }
        }

        private void Load(string path, string folder = null, bool asset = false)
        {
            if (string.IsNullOrEmpty(path)) return;

            items.Clear();

            if (Directory.Exists(path))
            {
                DirectoryInfo root = new DirectoryInfo(path);

                foreach (FileInfo file in root.GetFiles("*.*", SearchOption.AllDirectories))
                {
                    if (!IgnoreExtensionList.Contains(file.Extension))
                    {
                        if (!string.IsNullOrEmpty(folder) && folder.EndsWith(file.Name)) { }
                        else
                        {
                            items.Add(new ItemFile()
                            {
                                name = file.Name,
                                path = asset ? AssetPath(file.FullName) : file.FullName,
                                folder = folder,
                                size = file.Length,
                                select = true,
                            });
                        }
                    }
                }
            }
        }

        private void LoadAll(string path, string folder = null, string predicate = null, bool asset = false)
        {
            if (string.IsNullOrEmpty(path)) return;

            items.Clear();

            if (Directory.Exists(path))
            {
                List<ItemFile> files = new List<ItemFile>();

                Find(path, folder, asset, ref files);

                if (string.IsNullOrEmpty(predicate))
                {
                    items.AddRange(files);
                }
                else
                {
                    items.AddRange(files.FindAll(x => x.name.Contains(predicate)));
                }
            }
        }

        private void Find(string path, string type, bool asset, ref List<ItemFile> files)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo root = new DirectoryInfo(path);

                foreach (FileInfo file in root.GetFiles())
                {
                    if (!IgnoreExtensionList.Contains(file.Extension))
                    {
                        if (!string.IsNullOrEmpty(type) && type.EndsWith(file.Name)) { }
                        else
                        {
                            files.Add(new ItemFile()
                            {
                                name = file.Name,
                                path = asset ? AssetPath(file.FullName) : file.FullName,
                                folder = type,
                                size = file.Length,
                                select = true,
                            });
                        }
                    }
                }

                foreach (DirectoryInfo dir in root.GetDirectories())
                {
                    Find(dir.FullName, string.Format("{0}/{1}", type, dir.Name), asset, ref files);
                }
            }
        }

        private void BuildAssetBundle()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].select)
                {
                    BuildAssetBundle(items[i]);
                }
            }
            UpdateAssetBundleFolder();

            EditorUtility.DisplayDialog("Tips", "Build AssetBundle Success!", "Next");
        }

        private void BuildAssetBundle(ItemFile file)
        {
            string path = string.Format("{0}/{1}", AssetBundlePath, file.folder);

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            List<AssetBundleBuild> builds = new List<AssetBundleBuild>();

            AssetBundleBuild build = new AssetBundleBuild()
            {
                assetBundleName = Path.GetFileNameWithoutExtension(file.name),
                assetNames = new string[] { file.path }
            };
            builds.Add(build);

            BuildPipeline.BuildAssetBundles(path, builds.ToArray(), BuildAssetBundleOptions.None, BuildTarget.Android);
        }

        private void Upload()
        {
            if (EditorUtility.DisplayDialog("Tips", "Upload Assets Success!", ToLanguage("Upload MD5"), ToLanguage("Complete")))
            {
                UploadMd5();
            }
        }

        private void UploadMd5()
        {
            string path = HistoryPath;

            if (File.Exists(path))
            {
                Recompilation(path);

                ShowNotification(new GUIContent("Upload Md5 Success!"));
            }
        }

        private void OpenFileMd5()
        {
            string path = HistoryPath;

            if (File.Exists(path))
            {
                EditorUtility.OpenWithDefaultApp(path);
            }
        }

        private void OpenFolder(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            if (Directory.Exists(path))
            {
                path = path.Replace("/", "\\");

                System.Diagnostics.Process.Start("explorer.exe", path);
            }
            else
            {
                UnityEngine.Debuger.LogError(Author.Editor, "No Directory: " + path);
            }
        }

        public static void Record(string path, string root, List<ItemFile> items)
        {
            string key, value;

            if (File.Exists(path))
            {
                StreamWriter writer = new StreamWriter(path, true);

                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].select)
                    {
                        key = root + items[i].folder + "/" + items[i].name;

                        value = Utility.MD5.ComputeFile(items[i].path);

                        writer.WriteLine(string.Format("{0}|{1}", key, value));
                    }
                }
                writer.Dispose();
            }
            else
            {
                using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    StreamWriter writer = new StreamWriter(stream);

                    for (int i = 0; i < items.Count; i++)
                    {
                        if (items[i].select)
                        {
                            key = root + items[i].folder + "/" + items[i].name;

                            value = Utility.MD5.ComputeFile(items[i].path);

                            writer.WriteLine(string.Format("{0}|{1}", key, value));
                        }
                    }
                    writer.Dispose();
                };
            }
        }

        public static void Recompilation(string path)
        {
            if (!File.Exists(path)) return;

            Dictionary<string, string> history = new Dictionary<string, string>();

            List<string> lines = new List<string>();

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                StreamReader reader = new StreamReader(stream);

                string line = reader.ReadLine();

                string[] param = new string[2];

                while (!string.IsNullOrEmpty(line))
                {
                    param = line.Split('|');

                    if (param.Length == 2)
                    {
                        if (history.ContainsKey(param[0]))
                        {
                            history[param[0]] = param[1];
                        }
                        else
                        {
                            history.Add(param[0], param[1]);
                        }
                    }
                    line = reader.ReadLine();
                }
            }
            foreach (var line in history)
            {
                lines.Add(string.Join("|", line.Key, line.Value));
            }
            lines.Sort((a, b) =>
            {
                return a.CompareTo(b);
            });
            File.WriteAllLines(path, lines);
        }

        private string AssetPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;

            path = path.Replace("\\", "/");

            if (path.Contains("Assets/"))
            {
                int index = path.LastIndexOf("Assets/");

                path = path.Remove(0, index);
            }
            return path;
        }

        private string SourcePath
        {
            get
            {
                return string.Format("{0}/{1}", Application.dataPath, Game.Const.AssetPath.Package);
            }
        }

        private string AssetBundlePath
        {
            get
            {
                return string.Format("{0}/{1}/{2}", Utility.Path.Project, ResourceConfig.AssetBundle, ResourceConfig.Platform);
            }
        }

        private string HistoryPath
        {
            get
            {
                return string.Format("{0}/{1}", AssetBundlePath, ResourceConfig.Record);
            }
        }
    }
}