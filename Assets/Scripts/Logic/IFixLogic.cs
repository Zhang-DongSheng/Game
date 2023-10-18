using Game.Resource;
using IFix.Core;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace Game
{
    public class IFixLogic : Singleton<IFixLogic>, ILogic
    {
        private const string Folder = "Package/IFix";

        private readonly List<string> _assemblies = new List<string>()
        {
            "Assembly-CSharp.patch.bytes",
        };

        public void Initialize()
        {
            Load();
        }

        public void Release()
        {

        }

        private void Load()
        {
            VirtualMachine.Info = (s) => UnityEngine.Debug.Log(s);
            var sw = Stopwatch.StartNew();
            foreach (var assembly in _assemblies)
            {
                var patch = ResourceManager.Load<TextAsset>(string.Format("{0}/{1}", Folder, assembly));
                if (patch != null)
                {
                    Debuger.Log(Author.Resource, $"loading {assembly} ...");
                    PatchManager.Load(new MemoryStream(patch.bytes));
                }
            }
            UnityEngine.Debug.Log("loaded patch, using " + sw.ElapsedMilliseconds + " ms");
        }
    }
}