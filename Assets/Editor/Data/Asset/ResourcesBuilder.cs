using System.Collections.Generic;
using System.IO;
using Utils;
using UnityEngine;
using UnityEngine.Factory;

namespace UnityEditor
{
    public class ResourcesBuilder : EditorWindow
    {
        private FactoryConfig config;

        private List<Node> nodes;

        private Vector2 scroll;

        private readonly GUIStyle style_folder = new GUIStyle();

        private readonly GUIStyle style_file = new GUIStyle();

        [MenuItem("Data/Resources")]
        private static void Open()
        {
            ResourcesBuilder window = EditorWindow.GetWindow<ResourcesBuilder>();
            window.titleContent = new GUIContent("Resources Builder");
            window.minSize = new Vector2(500, 300);
            window.Init();
            window.Show();
        }

        private void Init()
        {
            TextAsset asset = Resources.Load<TextAsset>(FactoryConfig.XML);

            if (asset != null)
            {
                config = JsonUtility.FromJson<FactoryConfig>(asset.text);
            }
            else
            {
                config = new FactoryConfig();
            }

            nodes = Finder.Find(Application.dataPath + "/Resources");

            int index = nodes.FindIndex(x => x.name == FactoryConfig.XML);

            if (index != -1) nodes.RemoveAt(index);

            for (int i = 0; i < nodes.Count; i++)
            {
                Select(nodes[i]);
            }

            style_folder.normal.textColor = Color.blue;

            style_folder.fontStyle = FontStyle.Bold;

            style_folder.fontSize = 20;

            style_file.normal.textColor = Color.red;
        }

        private void Select(Node node)
        {
            if (node.type == NodeType.Folder)
            {
                NodeFolder folder = node as NodeFolder;

                for (int i = 0; i < folder.nodes.Count; i++)
                {
                    Select(folder.nodes[i]);
                }
            }
            else
            {
                NodeFile file = node as NodeFile;

                file.select = config.prefabs.Exists(x => x.key == node.name);
            }
        }

        private void OnGUI()
        {
            RefreshUI();
        }

        private void RefreshUI()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Assets > Resources");
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Box(string.Empty, GUILayout.ExpandWidth(true), GUILayout.Height(3));

                    scroll = GUILayout.BeginScrollView(scroll);
                    {
                        for (int i = 0; i < nodes.Count; i++)
                        {
                            RefreshNode(nodes[i]);
                        }
                    }
                    GUILayout.EndScrollView();
                }
                GUILayout.EndVertical();

                GUILayout.Box(string.Empty, GUILayout.Width(3), GUILayout.ExpandHeight(true));

                GUILayout.BeginVertical(GUILayout.Width(100));
                {
                    GUILayout.Space(30);

                    if (GUILayout.Button("刷新", GUILayout.Height(30)))
                    {
                        Init();
                    }

                    if (GUILayout.Button("生成XML", GUILayout.Height(20)))
                    {
                        Build();
                    }

                    if (GUILayout.Button("打开XML", GUILayout.Height(20)))
                    {
                        System.Diagnostics.Process.Start("notepad.exe", XML);
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private void RefreshNode(Node node)
        {
            switch (node.type)
            {
                case NodeType.Folder:
                    RefreshFolder(node as NodeFolder);
                    break;
                case NodeType.File:
                    RefreshFile(node as NodeFile);
                    break;
            }
        }

        private void RefreshFolder(NodeFolder node)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(20));
            {
                for (int i = 0; i < node.order; i++)
                {
                    GUILayout.Space(20);
                }
                if (GUILayout.Button(node.status ? "▲" : "▼", GUILayout.Width(30)))
                {
                    node.status = !node.status;
                }
                GUILayout.Label(string.Format("{0} >", Format(node.name)), style_folder);
            }
            GUILayout.EndHorizontal();

            if (node.status)
            {
                for (int i = 0; i < node.nodes.Count; i++)
                {
                    RefreshNode(node.nodes[i]);
                }
            }
        }

        private void RefreshFile(NodeFile node)
        {
            if (node.extension == ".meta") return;

            GUILayout.BeginHorizontal();
            {
                for (int i = 0; i < node.order; i++)
                {
                    GUILayout.Space(20);
                }
                GUILayout.Label(Format(node.path), style_file);

                node.name = GUILayout.TextField(node.name, GUILayout.Width(100));

                node.select = GUILayout.Toggle(node.select, "", GUILayout.Width(20));
            }
            GUILayout.EndHorizontal();
        }

        private void Build()
        {
            config.prefabs.Clear();

            for (int i = 0; i < nodes.Count; i++)
            {
                Builder(nodes[i]);
            }

            string content = JsonUtility.ToJson(config);

            WriteXML(content);

            AssetDatabase.Refresh();
        }

        private void Builder(Node node)
        {
            if (node.type == NodeType.Folder)
            {
                NodeFolder folder = node as NodeFolder;

                for (int i = 0; i < folder.nodes.Count; i++)
                {
                    Builder(folder.nodes[i]);
                }
            }
            else
            {
                NodeFile file = node as NodeFile;

                if (file.select)
                {
                    config.prefabs.Add(new PrefabInformation()
                    {
                        key = file.name,
                        path = Format(file.path).Replace(file.extension, ""),
                        capacity = 100,
                        extension = file.extension,
                    });
                }
            }
        }

        private void WriteXML(string content)
        {
            string path = XML;

            if (File.Exists(path))
            {
                File.WriteAllText(path, content);
            }
            else
            {
                using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    StreamWriter writer = new StreamWriter(stream);
                    writer.Write(content);
                    writer.Dispose();
                }
                ShowNotification(new GUIContent("Write XML Done!"));
            }
        }

        private string Format(string path)
        {
            path = path.Replace("\\", "/");

            if (path.StartsWith(Path))
            {
                return path.Remove(0, Path.Length + 1);
            }
            return path;
        }

        private string Path
        {
            get
            {
                return Application.dataPath + "/" + "Resources";
            }
        }

        private string XML
        {
            get
            {
                return string.Format("{0}/{1}.txt", Path, FactoryConfig.XML);
            }
        }
    }
}