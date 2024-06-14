using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemStep : ItemBase
    {
        [SerializeField] private Button forward;

        [SerializeField] private Button back;

        [SerializeField] private Text label;

        [SerializeField] private bool loop;

        private int last, count;

        private int _index, index;

        public UnityEvent<int> onValueChanged;

        private readonly List<int> list = new List<int>();

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

            last = count - 1;

            index = Mathf.Clamp(select, 0, last);

            OnClick(index);
        }

        private void OnClick(int index)
        {
            if (count > index)
            {
                var value = list[index];

                label.text = value.ToString();

                onValueChanged?.Invoke(value);
            }
        }

        private void OnClickForward()
        {
            _index++;

            if (_index > last)
            {
                _index = loop ? 0 : last;
            }
            if (index == _index) return;

            index = _index;

            OnClick(index);
        }

        private void OnClickBack()
        {
            _index--;

            if (_index < 0)
            {
                _index = loop ? last : 0;
            }
            if (index == _index) return;

            index = _index;

            OnClick(index);
        }
    }
}