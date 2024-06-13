using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.UI
{
    public class ActivityData
    {
        public uint activityID;

        public long start;

        public long end;

        public bool limited;

        public ActivityData()
        { 
        
        }

        public ActivityData(ActivityInformation activity)
        {
            this.activityID = activity.activityID;

            this.limited = activity.timeLimit;

            this.start = activity.beginTime;
            
            this.end = activity.endTime;
        }
    }
}