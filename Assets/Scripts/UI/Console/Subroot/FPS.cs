using UnityEngine;

namespace Game
{
    internal class FPS : MonoBehaviour
    {
        private const int capacity = 10;

        private int present;

        private int average, total;

        private int index;

        private readonly int[] samples = new int[capacity];

        private void Awake()
        {
            
        }

        private void Update()
        {
            present = (int)(1 / Time.deltaTime);

            if (index++ >= capacity - 1)
            {
                index = 0;
            }
            samples[index] = present;

            total = 0;

            for (int i = 0; i < capacity; i++)
            {
                total += samples[index];
            }
            average = total / capacity;
        }

        public int Fps => average;
    }
}