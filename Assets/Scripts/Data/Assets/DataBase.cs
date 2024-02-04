using Game;
using LitJson;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// 数据基类
    /// </summary>
    public abstract class DataBase : ScriptableObject
    {
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