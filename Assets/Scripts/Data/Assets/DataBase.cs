using UnityEngine;

namespace Data
{
    /// <summary>
    /// 数据基类
    /// </summary>
    public abstract class DataBase : ScriptableObject
    {
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