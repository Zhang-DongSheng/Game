using Game;
using LitJson;
using UnityEngine;

namespace Game.Data
{
    /// <summary>
    /// 数据基类
    /// </summary>
    public abstract class DataBase : ScriptableObject
    {
        [SerializeField] protected bool order;

        protected JsonData m_list = null;

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

        public virtual void Sort()
        {
            order = false;
        }

        public virtual void Clear()
        {

        }

        protected virtual void Editor()
        {

        }
        [ContextMenu("Editor")]
        protected void MenuEditor()
        {
            Editor();
        }
        [ContextMenu("Save")]
        protected void MenuSave()
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
    /// <summary>
    /// 信息基类
    /// </summary>
    public abstract class InformationBase
    {
        public uint primary;

        public static int Compare(InformationBase x, InformationBase y)
        {
            return x.primary.CompareTo(y.primary);
        }
    }
}