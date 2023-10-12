using Game.Attribute;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    public class RangeNumber : MonoBehaviour
    {
        [SerializeField, Readonly] private int display;

        [SerializeField] private int min;

        [SerializeField] private int max = 10;

        [SerializeField] private int step = 1;

        [SerializeField] private bool wrap;

        [SerializeField] private Button add;

        [SerializeField] private Button reduce;

        private int _value, value;

        public UnityEvent<int> onValueChanged;

        private void Awake()
        {
            add.onClick.AddListener(OnClickAdd);

            reduce.onClick.AddListener(OnClickReduce);
        }

        private void OnValidate()
        {
            Refresh();
        }

        private void Refresh()
        {
            display = value;

            if (wrap)
            {
                reduce.interactable = true;

                add.interactable = true;
            }
            else
            {
                reduce.interactable = value > min;

                add.interactable = value < max;
            }
        }

        private void OnClickAdd()
        {
            _value = value + step;

            if (wrap)
            {
                if (_value > max)
                {
                    _value = min + _value - max;
                }
            }
            else
            {
                _value = Mathf.Clamp(_value, min, max);
            }
            // 
            if (value != _value)
            {
                value = _value; onValueChanged?.Invoke(value);
            }
            Refresh();
        }

        private void OnClickReduce()
        {
            _value = value - step;

            if (wrap)
            {
                if (_value < min)
                {
                    _value = max - min + _value;
                }
            }
            else
            {
                _value = Mathf.Clamp(_value, min, max);
            }
            // 
            if (value != _value)
            {
                value = _value; onValueChanged?.Invoke(value);
            }
            Refresh();
        }
    }
}