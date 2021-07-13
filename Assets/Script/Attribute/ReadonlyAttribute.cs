namespace UnityEngine
{
    public class ReadonlyAttribute : PropertyAttribute
    {
        public bool editor;

        public ReadonlyAttribute(bool editor)
        {
            this.editor = editor;
        }
    }
}