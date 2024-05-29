namespace Game.UI
{
    public class Prop
    {
        public uint identification;

        public uint parallelism;

        public uint amount;

        public Prop(uint identification, uint parallelism, uint amount)
        {
            this.identification = identification;

            this.parallelism = parallelism;

            this.amount = amount;
        }
    }
}