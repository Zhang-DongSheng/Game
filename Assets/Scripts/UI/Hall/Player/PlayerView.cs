using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class PlayerView : ViewBase
    {
        [SerializeField] private ItemStep m_step;

        [SerializeField] private GraphicsBuffer m_attribute;

        [SerializeField] private List<ItemProperty> properties;

        protected override void OnAwake()
        {
            m_step.onValueChanged.AddListener(OnClickStep);
        }

        public override void Refresh(UIParameter parameter)
        {
            m_step.Refresh(new List<int>() { 1, 2 }, 0);
        }

        private void Refresh()
        { 
            
        }

        private void OnClickStep(int index)
        {
            //ModelManager.Instance.Modify(character, "shoes", index.ToString(), "blue");
        }
    }
}