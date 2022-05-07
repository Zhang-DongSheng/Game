using UnityEngine;

namespace Data
{
    /// <summary>
    /// ���ݻ���
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
    /// ��Ϣ����
    /// </summary>
    public abstract class InformationBase
    {
        public uint primary;
    }
}