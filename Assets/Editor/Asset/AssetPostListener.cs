using UnityEngine;

namespace UnityEditor
{
    class AssetPostListener : AssetPostprocessor
    {
#if DEBUG
        /// <summary>
        /// 导入声音前
        /// </summary>
        public void OnPreprocessAudio()
        {
            AudioImporter importer = assetImporter as AudioImporter;
        }
        /// <summary>
        /// 导入声音后
        /// </summary>
        /// <param name="clip">音频</param>
        public void OnPostprocessAudio(AudioClip clip)
        {
            Debug.Log("音频导入完成！");
        }
        /// <summary>
        /// 导入模型前
        /// </summary>
        public void OnPreprocessModel()
        {
            ModelImporter importer = assetImporter as ModelImporter;
        }
        /// <summary>
        /// 导入模型后
        /// </summary>
        /// <param name="gameObject">模型</param>
        public void OnPostprocessModel(GameObject gameObject)
        {
            Debug.Log("模型导入完成！");
        }
        /// <summary>
        /// 导入贴图前
        /// </summary>
        public void OnPreprocessTexture()
        {
            TextureImporter importer = assetImporter as TextureImporter;
        }
        /// <summary>
        /// 导入贴图后
        /// </summary>
        /// <param name="texture2D"></param>
        public void OnPostprecessTexture(Texture2D texture2D)
        {
            Debug.Log("贴图导入完成！");
        }
        /// <summary>
        /// 导入动画前
        /// </summary>
        public void OnPreprocessAnimation()
        {
            ModelImporter importer = assetImporter as ModelImporter;
        }
        /// <summary>
        /// 导入动画后
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="animationClip"></param>
        public void OnPostprocessAnimation(GameObject gameObject, AnimationClip animationClip)
        {
            Debug.Log("动画导入完成！");
        }
        /// <summary>
        /// 导入材质后
        /// </summary>
        /// <param name="material"></param>
        public void OnPostprocessMaterial(Material material)
        {
            Debug.Log("材质导入完成！");
        }
        /// <summary>
        /// 导入精灵后
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="sprites"></param>
        public void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
        {
            Debug.Log("图片导入完成！");
        }
        /// <summary>
        /// 资源变更
        /// </summary>
        /// <param name="importedAsset"></param>
        /// <param name="deletedAssets"></param>
        /// <param name="movedAssets"></param>
        /// <param name="movedFromAssetPaths"></param>
        public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string str in importedAsset)
            {
                Debug.LogFormat("新增 : <color=green>{0}</color>", str);
            }
            foreach (string str in deletedAssets)
            {
                Debug.LogFormat("删除 : <color=gray>{0}</color>", str);
            }
            foreach (string str in movedAssets)
            {
                Debug.LogFormat("移动 Start : <color=yellow>{0}</color>", str);
            }
            foreach (string str in movedFromAssetPaths)
            {
                Debug.LogFormat("移动 End : <color=blue>{0}</color>", str);
            }
        }
#endif
    }
}