using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ConsoleGameState : ConsoleBase
    {
        public override string Name => "状态";

        [SerializeField] private List<ItemConsoleLabel> labels;

        private const int capacity = 10;

        private int present, fps;

        private int average, total;

        private int index;

        private readonly int[] samples = new int[capacity];

        public override void Refresh(float delta)
        {
            fps = ComputeFPS(delta);

            labels[0].Refresh("FPS", $"{fps}");
        }

        private int ComputeFPS(float delta)
        {
            present = (int)(1 / delta);

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

            return average;
        }
    }
}