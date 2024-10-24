using Game;
using LitJson;
using UnityEngine;

namespace Game.Data
{
    /// <summary>
    /// ���ݻ���
    /// </summary>
    public abstract class DataBase : ScriptableObject
    {
        [SerializeField] protected bool order;

        protected JsonData m_list = null;

        public virtual void Load(string content)
        {
            Clear();
            // һ��Ҫ�ǵ�ȥ�����һ�еĶ���
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
    /// ��Ϣ����
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