using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// ∂‘œÛ
        /// </summary>
        public static class Object
        {
            public static void Unload(UnityEngine.Object asset)
            {
                if (asset == null) return;

                if (asset is Shader) return;

                if (asset is AssetBundle bundle)
                {
                    bundle.Unload(true);
                }
                else if (asset is UnityEngine.GameObject)
                {
                    UnityEngine.Object.Destroy(asset);
                }
                else if (asset is Component component)
                {
                    UnityEngine.Object.Destroy(component.gameObject);
                }
                else
                {
                    Resources.UnloadAsset(asset);
                }
            }
        }
    }
}