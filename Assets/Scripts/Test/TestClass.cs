using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Test
{
    public class TestClass
    {
        public void Print1()
        {
            Debuger.Log(Author.Test, "测试1");
        }

        public void Print2()
        {
            Debuger.Log(Author.Test, "测试2");
        }
    }
}
