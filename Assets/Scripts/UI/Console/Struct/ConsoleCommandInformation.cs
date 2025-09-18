namespace Game.Data
{
    public class ConsoleCommandInformation
    {
        public int index;

        public string name;

        public string command;

        public string description;

        public ConsoleCommandInformation(int index, string name, string command, string description)
        {
            this.index = index;

            this.name = name;

            this.command = command;

            this.description = description;
        }
    }
}