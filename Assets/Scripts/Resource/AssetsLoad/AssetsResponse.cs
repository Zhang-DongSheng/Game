using System;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;
using Object = UnityEngine.Object;

namespace Game.Resource
{
    public class AssetsResponse : IDisposable
    {
        public string name;

        public string path;

        public int reference;

        private Object[] _assets;

        private AssetBundle _assetBundle;

        private readonly long[] _size = new long[2] { 0, 0 };

        public AssetsResponse(string path)
        {
            this.name = Path.GetFileNameWithoutExtension(path);

            this.path = path;

            this.reference = 0;
        }

        public AssetBundle AssetBundle
        {
            get
            {
                return _assetBundle;
            }
            set
            {
                _assetBundle = value;

                _size[0] = 0;

                if (_assetBundle != null)
                {
                    _size[0] = Profiler.GetRuntimeMemorySizeLong(_assetBundle);
                }
            }
        }

        public Object[] Assets
        {
            get
            {
                return _assets;
            }
            set
            {
                _assets = value;

                _size[1] = 0;

                if (_assets != null)
                {
                    foreach (var item in _assets)
                    {
                        _size[1] += Profiler.GetRuntimeMemorySizeLong(item);
                    }
                }
            }
        }

        public Object MainAsset
        {
            get
            {
                if (_assets != null && _assets.Length > 0)
                {
                    return _assets[0];
                }
                return null;
            }
        }

        public long Size
        {
            get
            {
                return _size[1];
            }
        }

        public void Dispose()
        {
            if (_assetBundle != null)
            {
                _assetBundle.Unload(true);
            };
            _assets = null;
        }
    }
}