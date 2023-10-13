using Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.UI
{
    public abstract class UIActivityBase : ItemBase
    {
        public int activityID;

        public virtual void Refresh()
        { 
            
        }

        public bool Equal(int activity)
        {
            return this.activityID == activity;
        }
    }
}