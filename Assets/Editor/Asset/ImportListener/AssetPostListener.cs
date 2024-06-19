using UnityEngine;

namespace UnityEditor.Listener
{
    [InitializeOnLoad]
    class AssetPostListener : AssetPostprocessor
    {
        static AssetPostListener()
        {

        }
        /// <summary>
        /// 监听所有脚本编译完成
        /// </summary>
        [Callbacks.DidReloadScripts]
        static void AllScriptsReloaded()
        {
            
        }
        /// <summary>
        /// 在完成任意数量的资源导入后（当资源进度条到达末尾时）调用此函数
        /// </summary>
        public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string str in importedAsset)
            {
                if (str.StartsWith("Assets/"))
                {
                    Debuger.Log(Author.Resource, string.Format("新增 : <color=green>{0}</color>", str));
                }
            }
            foreach (string str in deletedAssets)
            {
                Debuger.Log(Author.Resource, string.Format("删除 : <color=gray>{0}</color>", str));
            }
            foreach (string str in movedAssets)
            {
                Debuger.Log(Author.Resource, string.Format("移动 Start : <color=yellow>{0}</color>", str));
            }
            foreach (string str in movedFromAssetPaths)
            {
                Debuger.Log(Author.Resource, string.Format("移动 End : <color=blue>{0}</color>", str));
            }
        }
        /// <summary>
        /// 当 AnimationClip 已完成导入时调用此函数
        /// </summary>
        public void OnPostprocessAnimation(GameObject target, AnimationClip clip)
        {

        }
        /// <summary>
        /// 将资源分配给其他资源捆绑包时调用的处理程序
        /// </summary>
        public void OnPostprocessAssetbundleNameChanged(string a, string b, string c)
        {
            //将此函数添加到一个子类中，以在资源的 assetBundle 名称发生更改后获取通知。
        }
        /// <summary>
        /// 将此函数添加到一个子类中，以在音频剪辑完成导入时获取通知
        /// </summary>
        public void OnPostprocessAudio(AudioClip clip)
        { }
        /// <summary>
        /// 将此函数添加到一个子类中，以在立方体贴图纹理完成导入之前获取通知
        /// </summary>
        public void OnPostprocessCubemap(Cubemap cubemap)
        {
            string lowerCaseAssetPath = assetPath.ToLower();

            if (lowerCaseAssetPath.IndexOf("/postprocessedcubemaps/") == -1)
                return;

            for (int m = 0; m < cubemap.mipmapCount; m++)
            {
                for (int face = 0; face < 6; face++)
                {
                    CubemapFace f = (CubemapFace)face;

                    Color[] c = cubemap.GetPixels(f, m);

                    for (int i = 0; i < c.Length; i++)
                    {
                        c[i].r = c[i].r * 0.5f;
                        c[i].g = c[i].g * 0.5f;
                        c[i].b = c[i].b * 0.5f;
                    }
                    cubemap.SetPixels(c, f, m);
                }
            }
        }
        /// <summary>
        /// 当自定义属性的动画曲线已完成导入时调用此函数
        /// </summary>
        public void OnPostprocessGameObjectWithAnimatedUserProperties(GameObject go, EditorCurveBinding[] curves) { }
        /// <summary>
        /// 为每个在导入文件中至少附加了一个用户属性的游戏对象调用此函数
        /// </summary>
        public void OnPostprocessGameObjectWithUserProperties(GameObject go, string[] properties, object[] paramter) { }
        /// <summary>
        /// 将此函数添加到一个子类中，以在材质资源完成导入时获取通知
        /// </summary>
        public void OnPostprocessMaterial(Material material)
        { }
        /// <summary>
        /// 当变换层级视图已完成导入时调用此函数
        /// </summary>
        public void OnPostprocessMeshHierarchy(GameObject go)
        { }
        /// <summary>
        /// 将此函数添加到一个子类中，以在模型完成导入时获取通知
        /// </summary>
        public void OnPostprocessModel(GameObject model)
        {
        
        }
        /// <summary>
        /// 将此函数添加到一个子类中，以在纹理刚完成导入之前获取通知
        /// </summary>
        public void OnPostprocessTexture(Texture2D texture)
        {
            if (texture.width % 4 != 0 || texture.height % 4 != 0)
            {
                Debuger.LogWarning(Author.Resource, "The Texture2D not power of 2!");
            }
        }
        /// <summary>
        /// 将此函数添加到一个子类中，以在导入模型（.fbx、.mb 文件等）中的动画之前获取通知
        /// </summary>
        public void OnPreprocessAnimation()
        {
            ModelImporter modelImporter = assetImporter as ModelImporter;
        }
        /// <summary>
        /// 将此函数添加到一个子类中，以在导入所有资源之前获取通知
        /// </summary>
        public void OnPreprocessAsset()
        {
            ModelImporter modelImporter = assetImporter as ModelImporter;
        }
        /// <summary>
        /// 将此函数添加到一个子类中，以在导入音频剪辑之前获取通知
        /// </summary>
        public void OnPreprocessAudio()
        {
            AudioImporter importer = assetImporter as AudioImporter;
        }
        /// <summary>
        /// 将此函数添加到一个子类中，以在导入模型（.fbx、.mb 文件等）之前获取通知
        /// </summary>
        public void OnPreprocessModel()
        {
            ModelImporter importer = assetImporter as ModelImporter;

            FBXListener.Start(importer);
        }
        /// <summary>
        /// 将此函数添加到一个子类中，以在导入 SpeedTree 资源（.spm 文件）之前获取通知
        /// </summary>
        public void OnPreprocessSpeedTree()
        {
            ModelImporter importer = assetImporter as ModelImporter;
        }
        /// <summary>
        /// 将此函数添加到一个子类中，以在纹理导入器运行之前获取通知
        /// </summary>
        public void OnPreprocessTexture()
        {
            TextureImporter importer = assetImporter as TextureImporter;

            TextureListener.Start(importer);
        }
    }
}