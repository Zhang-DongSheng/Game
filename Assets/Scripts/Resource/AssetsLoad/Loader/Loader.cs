using System;
using System.Collections;

namespace Game.Resource
{
    public abstract class Loader
    {
        public abstract AssetsData LoadAssets(string path);

        public abstract IEnumerator LoadAssetsAsync(string path, Action<AssetsData> callback);

        public virtual bool ExistDependencies(string name) { return false; }

        public virtual string[] GetAllDependenciesName(string name) { return new string[0]; }
    }
}