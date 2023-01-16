using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Window
{
    public partial class ArtistAsset : ArtistBase
    {
        private readonly string[] assetoptions = new string[6] { "None", "TextAsset", "Texture", "Sprite", "Shader", "Material" };

        private readonly string[] shaderoptions = new string[2] { "Reference", "Find" };

        private readonly string[] folders = new string[] { "Assets" };

        private readonly Index indexAsset = new Index();

        private readonly Index indexShader = new Index();

        private readonly Input inputShader = new Input();

        private readonly List<Object> assets = new List<Object>();

        public override string Name => "��Դ";
        
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
                        case 2:
                        case 3:
                            {
                                if (GUILayout.Button("���ͼƬ��Դ�Ƿ�Ϊ4�ı���"))
                                {
                                    Powerof2("Assets");
                                }
                                goto default;
                            }
                        case 4:
                            {
                                indexShader.value = EditorGUILayout.Popup(indexShader.value, shaderoptions);

                                switch (indexShader.value)
                                {
                                    case 0:
                                        {
                                            if (GUILayout.Button("��������δ�����õ�Shader"))
                                            {
                                                FindUnreferencedShader();
                                            }
                                            if (GUILayout.Button("�������б����õ�Shader"))
                                            {
                                                FindReferenceShader();
                                            }
                                        }
                                        break;
                                    case 1:
                                        {
                                            inputShader.value = GUILayout.TextField(inputShader.value);

                                            if (GUILayout.Button("�������ø���Դ������Material"))
                                            {
                                                FindMaterialOfReferenceShader(inputShader.value);
                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                        default:
                            {
                                if (GUILayout.Button("�����Դ����"))
                                {
                                    FindReferences.Empty(string.Format("t:{0}", assetoptions[indexAsset.value]), "Assets");
                                }
                                if (GUILayout.Button("�����Դ��С"))
                                {
                                    Overflow(string.Format("t:{0}", assetoptions[indexAsset.value]), "Assets");
                                }
                            }
                            break;
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }





        public void FindUnreferencedShader()
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
                    Debug.LogErrorFormat("The shader name is the same, {0}", shader);
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
                    Debug.LogWarning(string.Format("<color=red>{0}</color> is not Reference", shaders[i]), UnityEngine.Shader.Find(shaders[i]));
                }
            }
        }

        public void FindReferenceShader()
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
                    Debug.LogWarning(string.Format("<color=red>{0}</color> is Referenced", shaders[i]), UnityEngine.Shader.Find(shaders[i]));
                }
            }
        }

        public void FindMaterialOfReferenceShader(string shader)
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
                        Debug.LogWarning(string.Format("{0}: Material Shader is {1}!", path, shader), material);
                    }
                }
            }
        }

        public void Powerof2(params string[] folders)
        {
            string[] guids = AssetDatabase.FindAssets("t:Texture2D", folders);

            string path;

            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);

                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

                if (texture.width % 4 != 0 || texture.height % 4 != 0)
                {
                    Debug.LogWarning(string.Format("{0}: Not power of 2!", path), texture);
                }
            }

            guids = AssetDatabase.FindAssets("t:Sprite", folders);

            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);

                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

                if (sprite.texture.width % 4 != 0 || sprite.texture.height % 4 != 0)
                {
                    Debug.LogWarning(string.Format("{0}: Not power of 2!", path), sprite);
                }
            }
        }

        public void Overflow(string filter, params string[] folders)
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
                Debug.LogError(string.Format("{0} �ߴ糬��2048��", name), texture);
            }
            else if (texture.width > 1024 || texture.height > 1024)
            {
                Debug.LogWarning(string.Format("{0} �ߴ����1024��", name), texture);
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
                    Debug.LogError(string.Format("{0} δѹ����", name), texture);
                }
            }
        }

        private void OverflowMaterial(string name, Material material)
        {
            if (material.passCount > 2)
            {
                Debug.LogError(string.Format("{0} ��Ⱦ�������࣡", name), material);
            }
        }

        private void OverflowTextAsset(string name, TextAsset textAsset)
        {
            if (textAsset.bytes.Length > 1024 * 1024 * 1)
            {
                Debug.LogError(string.Format("{0} ���볬����", name), textAsset);
            }
        }

        
    }
}