using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemStep : ItemBase
    {
        [SerializeField] private Text content;

        [SerializeField] private Button forward;

        [SerializeField] private Button back;

        private readonly List<int> list = new List<int>();

        private int count, index;

        public UnityEvent<int> onValueChanged;

        protected override void OnAwake()
        {
            forward.onClick.AddListener(OnClickForward);

            back.onClick.AddListener(OnClickBack);
        }

        public void Refresh(List<int> list, int select = -1)
        {
            this.list.Clear();

            this.list.AddRange(list);

            count = list.Count;

            index = Mathf.Clamp(select, 0, count - 1);

            OnClick(index);
        }

        private void OnClick(int index)
        {
            if (count > index)
            {
                var value = list[index];

                content.text = value.ToString();

                onValueChanged?.Invoke(value);
            }
        }

        private void OnClickForward()
        {
            if (index < count - 1)
            {
                index++;
            }
            OnClick(index);
        }

        private void OnClickBack()
        {
            if (index > 0)
            {
                index--;
            }
            OnClick(index);
        }
    }
}
