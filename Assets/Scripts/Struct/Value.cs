using System;

namespace Game
{
    /// <summary>
    /// 布尔型
    /// </summary>
    [Serializable]
    public class BoolValue
    {
        public Action<bool> action;

        public bool value;

        public bool Value
        {
            get
            {
                return value;
            }
            set
            {
                if (this.value != value)
                {
                    action?.Invoke(value);
                }
                this.value = value;
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

        public int value;

        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                if (this.value != value)
                {
                    action?.Invoke(value);
                }
                this.value = value;
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

        public float value;

        public float Value
        {
            get
            {
                return value;
            }
            set
            {
                if (this.value != value)
                {
                    action?.Invoke(value);
                }
                this.value = value;
            }
        }
    }
    /// <summary>
    /// 泛型
    /// </summary>
    [Serializable]
    public class GenericityValue<T> where T : class
    {
        public Action<T> action;

        public T value;

        public T Value
        {
            get
            {
                return value;
            }
            set
            {
                if (this.value != value)
                {
                    action?.Invoke(value);
                }
                this.value = value;
            }
        }
    }
}