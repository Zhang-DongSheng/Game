namespace Data
{
    public class Task
    {        public uint identification;

        public uint parallelism;

        public float progress;

        public Status status;        public Task()
        {

        }        public Task(TaskInformation task)
        {
            identification = 0;

            parallelism = task.primary;

            progress = 0;

            status = Status.Undone;
        }    }
}