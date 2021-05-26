using System.Collections;
using System.Text;

namespace UnityEngine.DataStruct
{
    public class DString : IComparer
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

        public int Compare(object x, object y)
        {
            return 0;
        }

        public override string ToString()
        {
            return value;
        }
    }
}