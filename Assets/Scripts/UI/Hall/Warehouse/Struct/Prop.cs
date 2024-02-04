namespace Data
{
    public class Prop
    {
        public uint identification;

        public uint parallelism;

        public int amount;

        public Prop(uint identification, uint parallelism, int amount)
        {
            this.identification = identification;

            this.parallelism = parallelism;

            this.amount = amount;
        }
    }
}