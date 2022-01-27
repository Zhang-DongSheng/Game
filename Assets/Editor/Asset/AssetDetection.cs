using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    public class AssetDetection
    {
        private static readonly string[] folders = new string[] { "Assets" };

        public static bool Missing(GameObject go)
        {
            bool missing = false;

            Component[] components = go.GetComponentsInChildren<Component>(true);

            foreach (var component in components)
            {
                if (component != null)
                {
                    SerializedObject so = new SerializedObject(component);

                    SerializedProperty sp = so.GetIterator();

                    while (sp.NextVisible(true))
                    {
                        if (sp.propertyType == SerializedPropertyType.ObjectReference &&
                            sp.objectReferenceInstanceIDValue != 0 &&
                            sp.objectReferenceValue == null)
                        {
                            Debug.LogErrorFormat("{0} Missing : {1}", go.name, sp.propertyPath);
                        }
                    }
                }
                else
                {
                    missing = true;
                }
            }
            return missing;
        }

        public static void FindUnreferencedShader()
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

        public static void FindReferenceShader()
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

        public static void FindMaterialOfReferenceShader(string shader)
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

        public static void Powerof2(params string[] folders)
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

        public static void Overflow(string filter, params string[] folders)
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

        protected static void OverflowTexture(string name, Texture2D texture)
        {
            if (texture.width > 2048 || texture.height > 2048)
            {
                Debug.LogError(string.Format("{0} ³ß´ç³¬³ö2048£¡", name), texture);
            }
            else if (texture.width > 1024 || texture.height > 1024)
            {
                Debug.LogWarning(string.Format("{0} ³ß´ç´óÓÚ1024£¡", name), texture);
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
                    Debug.LogError(string.Format("{0} Î´Ñ¹Ëõ£¡", name), texture);
                }
            }
        }

        protected static void OverflowMaterial(string name, Material material)
        {
            if (material.passCount > 2)
            {
                Debug.LogError(string.Format("{0} äÖÈ¾´ÎÊý¹ý¶à£¡", name), material);
            }
        }

        protected static void OverflowTextAsset(string name, TextAsset textAsset)
        {
            if (textAsset.bytes.Length > 1024 * 1024 * 1)
            {
                Debug.LogError(string.Format("{0} ´úÂë³¬³¤£¡", name), textAsset);
            }
        }
    }
}