using System;

namespace Game
{
    /// <summary>
    /// 布尔型
    /// </summary>
    [Serializable]
    public sealed class BoolValue
    {
        public Action<bool> action;

        private bool _value = false;

        public bool value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!_value.Equals(value))
                {
                    action?.Invoke(value);
                }
                _value = value;
            }
        }
    }
    /// <summary>
    /// 整形
    /// </summary>
    [Serializable]
    public class IntValue
    {
        public Action<int> action;

        private int _value = 0;

        public int value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!_value.Equals(value))
                {
                    action?.Invoke(value);
                }
                _value = value;
            }
        }
    }
    /// <summary>
    /// 浮点型
    /// </summary>
    [Serializable]
    public class FloatValue
    {
        public Action<float> action;

        private float _value = 0;

        public float value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!_value.Equals(value))
                {
                    action?.Invoke(value);
                }
                _value = value;
            }
        }
    }
    /// <summary>
    /// Class泛型
    /// </summary>
    [Serializable]
    public class ClassValue<T> where T : class
    {
        public Action<T> action;

        private T _value = default;

        public T value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!_value.Equals(value))
                {
                    action?.Invoke(value);
                }
                _value = value;
            }
        }
    }
    /// <summary>
    /// Struct泛型
    /// </summary>
    [Serializable]
    public class StructValue<T> where T : struct
    {
        public Action<T> action;

        private T _value = default;

        public T value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!_value.Equals(value))
                {
                    action?.Invoke(value);
                }
                _value = value;
            }
        }
    }
    /// <summary>
    /// 基类型
    /// </summary>
    [Serializable]
    public class ObjectValue
    {
        public Action<object> action;

        private object _value = null;

        public object value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!_value.Equals(value))
                {
                    action?.Invoke(value);
                }
                _value = value;
            }
        }
    }
}