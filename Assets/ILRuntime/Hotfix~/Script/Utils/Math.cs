using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILRuntime.Game
{
    class Math
    {
        public static float Add(float a, float b)
        {
            return a + b;
        }

        public static float Subtract(float a, float b)
        {
            return a - b;
        }

        public static float Multiply(float a, float b)
        {
            return a * b;
        }

        public static float Divide(float a, float b)
        {
            return a / b;
        }

        public static bool Equal(float a, float b)
        {
            return a == b;
        }
    }
}