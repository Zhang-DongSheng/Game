using Game;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using Debuger = UnityEngine.Debuger;

namespace UnityEditor.Window
{
    public partial class ArtistAsset : ArtistBase
    {
        private readonly string[] assetoptions = new string[7] { "None", "TextAsset", "Texture", "Sprite", "Shader", "Material", "Model" };

        private readonly string[] shaderoptions = new string[2] { "Reference", "Find" };

        private readonly string[] folders = new string[] { "Assets" };

        private readonly Index indexAsset = new Index();

        private readonly Index indexShader = new Index();

        private readonly Input inputShader = new Input();

        private readonly List<Object> assets = new List<Object>();

        public override void Initialise()
        {
            indexAsset.action = (index) =>
            {
                assets.Clear();

                string[] guids = AssetDatabase.FindAssets("t:Shader", folders);

                foreach (var guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);

                    var asset = AssetDatabase.LoadAssetAtPath<Object>(path);

                    assets.Add(asset);
                }
            };
        }

        public override void Refresh()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    indexAsset.value = EditorGUILayout.Popup(indexAsset.value, assetoptions);
                }
                GUILayout.EndVertical();

                GUILayout.Box(string.Empty, GUILayout.Width(3), GUILayout.ExpandHeight(true));

                GUILayout.BeginVertical(GUILayout.Width(200));
                {
                    switch (indexAsset.value)
                    {
                        case 0:
                            {
                                GUILayout.Label(string.Empty);
                            }
                            break;
                        case 1: // TextAsset
                            {
                                if (GUILayout.Button("Detection Empty Function"))
                                {
                                    DetectionEmptyFunction();
                                }
                                // 检测中文
                                if (GUILayout.Button("Detection Chinese"))
                                {
                                    DetectionChinese();
                                }
                                // 检测引号
                                if (GUILayout.Button("Detection Quotation Marks"))
                                {
                                    DetectionString();
                                }
                                // 检测代码大小
                                if (GUILayout.Button("Detection Code Size"))
                                {
                                    Overflow(string.Format("t:{0}", assetoptions[indexAsset.value]), "Assets");
                                }
                            }
                            break;
                        case 2: // Texture
                        case 3: // Sprite
                            {
                                if (GUILayout.Button("The sprite size is a multiple of 4"))
                                {
                                    Powerof2("Assets");
                                }
                                if (GUILayout.Button("Detection Texture Reference"))
                                {
                                    FindReferences.Empty(string.Format("t:{0}", assetoptions[indexAsset.value]), "Assets");
                                }
                                if (GUILayout.Button("Detection Texture Size"))
                                {
                                    Overflow(string.Format("t:{0}", assetoptions[indexAsset.value]), "Assets");
                                }
                                break;
                            }
                        case 4: // Shader
                            {
                                indexShader.value = EditorGUILayout.Popup(indexShader.value, shaderoptions);

                                switch (indexShader.value)
                                {
                                    case 0:
                                        {
                                            if (GUILayout.Button("Find all Unreferenced Shader"))
                                            {
                                                FindUnreferencedShader();
                                            }
                                            if (GUILayout.Button("Find all Referenced Shader"))
                                            {
                                                FindReferenceShader();
                                            }
                                        }
                                        break;
                                    case 1:
                                        {
                                            inputShader.value = GUILayout.TextField(inputShader.value);

                                            if (GUILayout.Button("Find all Referenced Material"))
                                            {
                                                FindMaterialOfReferenceShader(inputShader.value);
                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                        case 5: // 
                            {
                                if (GUILayout.Button(ToLanguage("Detection Material Reference")))
                                {
                                    FindReferences.Empty(string.Format("t:{0}", assetoptions[indexAsset.value]), "Assets");
                                }
                            }
                            break;
                        case 6: // Model
                            break;
                        default:
                            break;
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private void FindUnreferencedShader()
        {
            List<string> shaders = new List<string>();

            string path, shader;

            int index, count;

            string[] guids = AssetDatabase.FindAssets("t:Shader", folders);

            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);

                shader = AssetDatabase.LoadAssetAtPath<Shader>(path).name;

                if (shaders.Contains(shader))
                {
                    UnityEngine.Debuger.LogError(Author.Editor, $"The shader name is the same, {shader}");
                }
                else
                {
                    shaders.Add(shader);
                }
            }

            guids = AssetDatabase.FindAssets("t:Material", folders);

            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);

                Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

                if (material.shader != null)
                {
                    shader = material.shader.name;

                    index = shaders.FindIndex(x => shader.Equals(x));

                    if (index > -1)
                    {
                        shaders.RemoveAt(index);
                    }
                }
            }

            count = shaders.Count;

            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(shaders[i]))
                {
                    UnityEngine.Debuger.LogWarning(Author.Editor, $"<color=red>{shaders[i]}</color> is not Reference", Shader.Find(shaders[i]));
                }
            }
        }

        private void FindReferenceShader()
        {
            List<string> shaders = new List<string>();

            string path, shader;

            string[] guids = AssetDatabase.FindAssets("t:Material", folders);

            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);

                Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

                if (material.shader != null)
                {
                    shader = material.shader.name;

                    if (shaders.Contains(shader)) { }
                    else
                    {
                        shaders.Add(shader);
                    }
                }
            }

            int count = shaders.Count;

            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(shaders[i]))
                {
                    UnityEngine.Debuger.LogWarning(Author.Editor, $"<color=red>{shaders[i]}</color> is Referenced", Shader.Find(shaders[i]));
                }
            }
        }

        private void FindMaterialOfReferenceShader(string shader)
        {
            string[] guids = AssetDatabase.FindAssets("t:Material", folders);

            string path;

            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);

                Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

                if (material.shader != null)
                {
                    string key = material.shader.name.ToLower();

                    if (key.Contains(shader.ToLower()))
                    {
                        UnityEngine.Debuger.LogWarning(Author.Editor, $"{path}: Material Shader is {shader}!", material);
                    }
                }
            }
        }

        private void DetectionEmptyFunction()
        {
            string[] guids = AssetDatabase.FindAssets("t:TextAsset", folders);

            string path, extension;

            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);

                extension = Path.GetExtension(path);

                switch (extension)
                {
                    case ".cs":
                        {
                            if (path.StartsWith("Assets/Scripts"))
                            {
                                TextAsset asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

                                if (!string.IsNullOrEmpty(asset.text))
                                {
                                    string pattern = @"void\s*Update\s*?\(\s*?\)\s*?\n*?\{\n*?\s*?\}";

                                    bool pass = Regex.IsMatch(asset.text, pattern);

                                    if (pass)
                                    {
                                        UnityEngine.Debuger.LogError(Author.Editor, $"代码{path}包含空方法!", asset);
                                    }
                                }
                            }
                            else
                            {
                                UnityEngine.Debuger.Log(Author.Editor, $"代码{path}暂不处理!");
                            }
                        }
                        break;
                    default:
                        {
                            UnityEngine.Debuger.Log(Author.Editor, $"其他格式文本{path}暂不处理!");
                        }
                        break;
                }
            }
        }

        private void DetectionChinese()
        {
            var files = Directory.GetFiles(Application.dataPath + "/Scripts", "*.cs", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var content = File.ReadAllText(file);

                var result = content.ContainsChinese();

                var count = result.Count;

                if (count > 0)
                {
                    var path = Utility.Path.SystemToUnity(file);

                    var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

                    var message = string.Join("\n", result);

                    UnityEngine.Debuger.LogError(Author.Editor, path + "\n" + message, asset);
                }
            }
        }

        private void DetectionString()
        {
            var files = Directory.GetFiles(Application.dataPath + "/Scripts", "*.cs", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var content = File.ReadAllText(file);

                var result = content.ContainsQuotationMarks();

                if (result.Count > 0)
                {
                    var path = Utility.Path.SystemToUnity(file);

                    var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

                    var message = string.Join("\n", result);

                    UnityEngine.Debuger.LogError(Author.Editor, path + "\n" + message, asset);
                }
            }
        }

        private void Powerof2(params string[] folders)
        {
            string[] guids = AssetDatabase.FindAssets("t:Texture2D", folders);

            string path;

            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);

                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

                if (texture.width % 4 != 0 || texture.height % 4 != 0)
                {
                    UnityEngine.Debuger.LogWarning(Author.Editor, $"{path}: Not power of 2!", texture);
                }
            }
            guids = AssetDatabase.FindAssets("t:Sprite", folders);

            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);

                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

                if (sprite.texture.width % 4 != 0 || sprite.texture.height % 4 != 0)
                {
                    UnityEngine.Debuger.LogWarning(Author.Editor, $"{path}: Not power of 2!", sprite);
                }
            }
        }

        private void Overflow(string filter, params string[] folders)
        {
            if (folders == null || folders.Length == 0) return;

            string[] guids = AssetDatabase.FindAssets(filter, folders);

            if (guids.Length == 0) return;

            string path; Object asset;

            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);

                asset = AssetDatabase.LoadAssetAtPath<Object>(path);

                if (asset == null)
                {
                    continue;
                }
                else if (asset is Texture2D texture)
                {
                    OverflowTexture(path, texture);
                }
                else if (asset is Material material)
                {
                    OverflowMaterial(path, material);
                }
                else if (asset is TextAsset text)
                {
                    OverflowTextAsset(path, text);
                }
            }
        }

        private void OverflowTexture(string name, Texture2D texture)
        {
            if (texture.width > 2048 || texture.height > 2048)
            {
                UnityEngine.Debuger.LogError(Author.Editor, $"{name} Size beyond 2048", texture);
            }
            else if (texture.width > 1024 || texture.height > 1024)
            {
                UnityEngine.Debuger.LogWarning(Author.Editor, $"{name} Size beyond 1024", texture);
            }
            else if (texture.format != TextureFormat.ASTC_4x4)
            {
#if UNITY_ANDROID
                if (texture.format == TextureFormat.ETC2_RGB ||
                    texture.format == TextureFormat.ETC2_RGBA8Crunched ||
                    texture.format == TextureFormat.ASTC_4x4)
#elif UNITY_IOS
                if (texture.format == TextureFormat.PVRTC_RGB4 ||
                    texture.format == TextureFormat.PVRTC_RGBA4 ||
                    texture.format == TextureFormat.ASTC_4x4)
#else
                if (texture.format != TextureFormat.RGBA32)
#endif
                {

                }
                else
                {
                    UnityEngine.Debuger.LogError(Author.Editor, $"{name} Uncompressed", texture);
                }
            }
        }

        private void OverflowMaterial(string name, Material material)
        {
            if (material.passCount > 2)
            {
                UnityEngine.Debuger.LogError(Author.Editor, $"{name} The Shader pass is too much", material);
            }
        }

        private void OverflowTextAsset(string name, TextAsset textAsset)
        {
            if (textAsset.bytes.Length > 1024 * 1024 * 1)
            {
                UnityEngine.Debuger.LogError(Author.Editor, $"{name} The code is super long", textAsset);
            }
        }

        public override string Name => ToLanguage("Assets");
    }
}