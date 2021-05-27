using System;
using System.Text;

namespace UnityEngine.DataStruct
{
    public class DString : IComparable<DString>
    {
        private readonly StringBuilder builder = new StringBuilder();

        public string value { get; private set; }

        public DString(string value)
        {
            this.value = value;
        }

        public static implicit operator DString(string value)
        {
            return new DString(value);
        }

        public static DString operator +(DString basic, string additive)
        {
            basic.builder.Clear();

            basic.builder.Append(basic.value);

            basic.builder.Append(additive);

            basic.value = basic.builder.ToString();

            return basic;
        }

        public static DString operator +(DString basic, DString additive)
        {
            basic.builder.Clear();

            basic.builder.Append(basic.value);

            basic.builder.Append(additive.value);

            basic.value = basic.builder.ToString();

            return basic;
        }

        public char this[int index]
        {
            get
            {
                if (value.Length > index)
                {
                    return value[index];
                }
                return default;
            }
        }

        public int CompareTo(DString target)
        {
            if (value == target.value)
            {
                return 0;
            }
            else
            {
                return value[0] > target[0] ? 1 : -1;
            }
        }

        public override int GetHashCode()
        {
            return string.Format("{0}#{1}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, value).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is DString target)
            {
                return value == target.value;
            }
            else if (obj is string value)
            {
                return this.value == value;
            }
            return false;
        }

        public override string ToString()
        {
            return value;
        }
    }
}