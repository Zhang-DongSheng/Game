using UnityEngine;
using UnityEngine.UI;

namespace Example
{
    public class ItemESA : DynamicInfinityItem
    {
        public Text m_TxtName;

        public Button m_Btn;

        private void Start()
        {
            m_Btn.onClick.AddListener(() =>
            {
                print("Click " + mData.ToString());
            });
        }

        protected override void OnRenderer()
        {
            base.OnRenderer();
            m_TxtName.text = mData.ToString();
        }
    }
}