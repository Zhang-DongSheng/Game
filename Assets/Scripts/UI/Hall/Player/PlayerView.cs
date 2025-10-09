using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class PlayerView : ViewBase
    {
        [SerializeField] private ItemStep m_step;

        [SerializeField] private ItemAttributes m_attribute;

        [SerializeField] private List<ItemProperty> properties;

        protected override void OnAwake()
        {
            m_step.onValueChanged.AddListener(OnClickStep);
        }

        public override void Refresh(UIParameter parameter)
        {
            m_step.Refresh(new List<int>() { 1, 2 }, 0);
        }

        private void Refresh(int index)
        { 
            var list = new List<float>()
            { 
                Random.Range(0, 1f),
                Random.Range(0, 1f),
                Random.Range(0, 1f),
                Random.Range(0, 1f),
                Random.Range(0, 1f),
            };
            m_attribute.Refresh(list);

            int count = list.Count;

            for (int i = 0; i < count; i++)
            {
                if (list.Count > i)
                {
                    properties[i].Refresh(i, list[i], 1);
                }
                else
                {
                    properties[i].SetActive(false);
                }
            }
        }

        private void OnClickStep(int index)
        {
            Refresh(index);
        }
    }
}