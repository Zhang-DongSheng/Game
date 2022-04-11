using System.IO;
using UnityEngine;
using UnityEngine.Profiling;

namespace Game.Resource
{
    public class AssetsData
    {
        public string name;

        public string path;

        public int reference;

        public long[] size = new long[2] { 0, 0 };

        private AssetBundle assetBundle;
        public AssetBundle AssetBundle
        {
            get
            {
                return assetBundle;
            }
            set
            {
                assetBundle = value;

                size[0] = 0;

                if (assetBundle != null)
                {
                    size[0] = Profiler.GetRuntimeMemorySizeLong(assetBundle);
                }
            }
        }

        private Object[] assets;
        public Object[] Assets
        {
            get
            {
                return assets;
            }
            set
            {
                assets = value;

                size[1] = 0;

                if (assets != null)
                {
                    foreach (var item in assets)
                    {
                        size[1] += Profiler.GetRuntimeMemorySizeLong(item);
                    }
                }
            }
        }

        public AssetsData(string path)
        {
            this.name = Path.GetFileNameWithoutExtension(path);

            this.path = path;

            this.reference = 0;
        }

        public Object GetAsset<T>() where T : Object
        {
            foreach (var item in assets)
            {
                if (item.GetType() == typeof(T))
                    return (T)item;
            }
            return default(T);
        }
    }
}