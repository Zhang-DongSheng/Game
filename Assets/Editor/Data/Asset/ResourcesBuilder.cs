using System.Collections.Generic;
using System.IO;
using Utils;
using UnityEngine;
using UnityEngine.Factory;

namespace UnityEditor
{
    public class ResourcesBuilder : EditorWindow
    {
        private const string XML = "config";

        private readonly FactoryConfig config = new FactoryConfig();

        private List<Node> nodes;

        private Vector2 scroll;

        private readonly GUIStyle style_folder = new GUIStyle();

        private readonly GUIStyle style_file = new GUIStyle();

        [MenuItem("Data/Resource")]
        private static void Open()
        {
            ResourcesBuilder window = EditorWindow.GetWindow<ResourcesBuilder>();
            window.titleContent = new GUIContent("Resources");
            window.minSize = new Vector2(500, 300);
            window.Init();
            window.Show();
        }

        private void Init()
        {
            nodes = Finder.Find(Application.dataPath + "/Resources");



            style_folder.normal.textColor = Color.blue;

            style_folder.fontStyle = FontStyle.Bold;

            style_folder.fontSize = 20;

            style_file.normal.textColor = Color.red;
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

                GUILayout.Space(5);

                GUILayout.BeginVertical(GUILayout.Width(100));
                {
                    GUILayout.Space(30);

                    if (GUILayout.Button("Build", GUILayout.Height(30)))
                    {
                        Build();
                    }

                    if (GUILayout.Button("Refresh"))
                    {
                        Init();
                    }

                    if (GUILayout.Button("Open Config"))
                    {
                        System.Diagnostics.Process.Start("notepad.exe", XMLPath);
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

                GUILayout.Label(string.Format("{0} >", Format(node.key)), style_folder);
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

                node.select = GUILayout.Toggle(node.select, "", GUILayout.Width(20));

                node.key = GUILayout.TextField(node.key, GUILayout.Width(100));
            }
            GUILayout.EndHorizontal();
        }

        private void Build()
        {
            config.prefabs.Clear();

            for (int i = 0; i < nodes.Count; i++)
            {
                Compute(nodes[i]);
            }

            string content = JsonUtility.ToJson(config);

            WriteXML(content);

            AssetDatabase.Refresh();
        }

        private void Compute(Node node)
        {
            if (node.type == NodeType.Folder)
            {
                NodeFolder folder = node as NodeFolder;

                for (int i = 0; i < folder.nodes.Count; i++)
                {
                    Compute(folder.nodes[i]);
                }
            }
            else
            {
                NodeFile file = node as NodeFile;

                if (file.select)
                {
                    config.prefabs.Add(new PrefabInformation()
                    {
                        key = file.key,
                        path = Format(file.path).Replace(file.extension, ""),
                    });
                }
            }
        }

        private string ReadXML()
        {
            if (File.Exists(XMLPath))
            {
                return File.ReadAllText(XMLPath);
            }
            return null;
        }

        private void WriteXML(string content)
        {
            if (File.Exists(XMLPath))
            {
                File.WriteAllText(XMLPath, content);
            }
            else
            {
                using (FileStream stream = new FileStream(XMLPath, FileMode.OpenOrCreate))
                {
                    StreamWriter writer = new StreamWriter(stream);
                    writer.Write(content);
                    writer.Dispose();
                }
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

        private string XMLPath
        {
            get
            {
                return string.Format("{0}/{1}.txt", Path, XML);
            }
        }
    }
}