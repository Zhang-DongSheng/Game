using Game.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class HotfixView : ViewBase
    {
        public List<HotfixComponent> components;

        private object script;

        public override void Init(UIInformation information)
        {
            base.Init(information);

            var name = $"Hotfix.UI.{information.name}";

            Type type = HotfixLogic.Instance.GetType(name);

            Debuger.LogError(Author.Script, $"╪сть{name} {type != null}");

            script = Activator.CreateInstance(type);

            script.Call("Initialise", transform);
        }

        public override void Refresh(UIParameter parameter)
        {
            script.Call("Refresh");
        }

        public override void Release()
        {
            if (script != null)
            {
                script.Call("Release");
            }
            script = null; base.Release();
        }
    }
}