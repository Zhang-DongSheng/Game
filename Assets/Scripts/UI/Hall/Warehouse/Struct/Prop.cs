namespace Game.Data
{
    public class Prop
    {
        public long identification;

        public uint parallelism;

        public uint amount;

        public Prop(long identification, uint parallelism, uint amount)
        {
            this.identification = identification;

            this.parallelism = parallelism;

            this.amount = amount;
        }
    }
}