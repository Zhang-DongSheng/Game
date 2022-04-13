using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        public static class Object
        {
            public static void Unload(UnityEngine.Object asset)
            {
                if (asset == null)
                {
                    return;
                }
                else if (asset is Shader)
                {
                    return;
                }

                if (asset is AssetBundle bundle)
                {
                    bundle.Unload(true);
                }
                else if (asset is GameObject || asset is Component)
                {
                    UnityEngine.Object.Destroy(asset);
                }
                else
                {
                    Resources.UnloadAsset(asset);
                }
            }
        }
    }
}