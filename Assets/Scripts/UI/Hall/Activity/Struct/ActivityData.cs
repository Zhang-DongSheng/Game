namespace Game.Data
{
    public class ActivityData
    {
        public uint activityID;

        public long start;

        public long end;

        public int limit;

        public ActivityData()
        { 
        
        }

        public ActivityData(ActivityInformation activity)
        {
            this.activityID = activity.activityID;

            this.limit = activity.limit;

            this.start = activity.start;
            
            this.end = activity.end;
        }
    }
}