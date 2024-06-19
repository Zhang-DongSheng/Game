using Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.UI
{
    public abstract class SubActivityBase : ItemBase
    {
        public uint activityID;

        public virtual void Refresh()
        { 
            
        }

        public bool Equal(uint activity)
        {
            return this.activityID == activity;
        }
    }
}