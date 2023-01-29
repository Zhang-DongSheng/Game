using UnityEngine;

namespace Data
{
    /// <summary>
    /// ���ݻ���
    /// </summary>
    public abstract class DataBase : ScriptableObject
    {
        public virtual void Set(string content)
        {
            Clear();
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
    /// ��Ϣ����
    /// </summary>
    public abstract class InformationBase
    {
        public uint primary;
    }
}