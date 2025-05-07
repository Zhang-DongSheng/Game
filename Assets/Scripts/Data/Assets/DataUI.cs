using Game.Const;
using Game.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    public class DataUI : DataBase
    {
        public List<UIInformation> list = new List<UIInformation>();

        public static UIInformation Get(int panel)
        {
            var data = DataManager.Instance.Load<DataUI>();

            if (data != null)
            {
                return data.list.Find(x => x.panel == panel);
            }
            return null;
        }

        public override void Detection()
        {
            var dic = new Dictionary<int, int>();

            int count = list.Count;

            for (int i = 0; i < count; i++)
            {
                if (dic.ContainsKey(list[i].panel))
                {
                    Debuger.LogError(Author.Data, "ui exist the same key:" + list[i].name);
                }
                else
                {
                    dic.Add(list[i].panel, 1);
                }
            }
        }

        public override void Clear()
        {
            list = new List<UIInformation>();
        }
    }
    [System.Serializable]
    public class UIInformation : InformationBase
    {
        public string name;

        public int panel;

        public UIType type;

        public UILayer layer;

        public uint order;

        public string path;

        public bool destroy;

        public UIInformation()
        {

        }

        public UIInformation(int panel, string name)
        {
            this.name = $"{name}View";

            this.panel = panel;

            type = UIType.Panel;

            layer = UILayer.Window;

            destroy = false;

            order = 0;

            path = string.Format("{0}/{1}View.prefab", AssetPath.UIPrefab, name);
        }

        public UIInformation(UIInformation other)
        {
            primary = other.primary;

            name = other.name;

            panel = other.panel;

            type = other.type;

            layer = other.layer;

            order = other.order;

            path = other.path;

            destroy = other.destroy;
        }
    }
}