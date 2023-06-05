namespace Game.Pool
{
    public class PoolReference
    {
        private int index, count;

        private int reference;

        private float timer, interval;

        private readonly int[] sample;

        public PoolReference(int sample, float interval)
        {
            this.count = sample;

            this.sample = new int[count];

            this.interval = interval;
        }

        public void Update(float delta, int number)
        {
            timer += delta;

            if (timer > interval)
            { 
                timer = 0;

                if (++index == count)
                {
                    index = 0;
                }
                sample[index] = number;

                reference = number;

                for (int i = 0; i < count; i++)
                {
                    if (reference < sample[i])
                    {
                        reference = sample[i];
                    }
                }
            }
        }

        public int Reference => reference;
    }
}