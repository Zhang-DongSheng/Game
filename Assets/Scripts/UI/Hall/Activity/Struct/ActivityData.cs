namespace Game.Data
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