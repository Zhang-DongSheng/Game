using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Develop
{
    public abstract class DevelopBase
    {
        public abstract string Name { get; }

        public abstract void Refresh();
    }
}