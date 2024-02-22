using Game;
using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// 数据基类
    /// </summary>
    public abstract class DataBase : ScriptableObject
    {
        protected JsonData m_list = null;

        protected readonly List<UIntPair> pairs = new List<UIntPair>();

        public virtual void Load(string content)
        {
            Clear();
            // 一定要记得去掉最后一行的逗号
            var json = JsonMapper.ToObject(content);

            if (json.ContainsKey("list"))
            {
                m_list = json.GetJson("list");
            }
            else
            {
                Debuger.LogError(Author.Data, string.Format("json con't have list! in {0}", this.name));
            }
        }

        public virtual void Detection()
        { 
            
        }

        public virtual void Sort(List<InformationBase> list)
        {
            int count = list.Count;

            list.Sort((x, y) =>
            {
                return x.primary.CompareTo(y.primary);
            });
        }

        public virtual void Divide(List<InformationBase> list)
        {
            pairs.Clear();

            Sort(list);

            int step = 0, interval = 100;

            int count = list.Count;

            for (int i = 0; i < count; i++)
            {
                if (--step < 0)
                {
                    pairs.Add(new UIntPair()
                    {
                        x = list[i].primary,

                        y = (uint)i
                    });
                    step = interval;
                }
            }
        }

        public virtual void Clear()
        {

        }
        [ContextMenu("Save")]
        protected void MenuSave()
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
        [ContextMenu("Editor")]
        protected void MenuEditor()
        {
            Editor();
        }
        protected virtual void Editor() { }
    }
    /// <summary>
    /// 信息基类
    /// </summary>
    public abstract class InformationBase
    {
        public uint primary;
    }
}