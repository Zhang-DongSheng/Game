using System;
using Unity.Collections;
using Unity.Jobs;

namespace Job
{
    public struct NumberJob : IJob, IDisposable
    {
        public NativeArray<float> numbers;

        public Command command;

        public NativeArray<float> result;

        private int count;

        public NumberJob(Command command, params float[] parameters)
        {
            this.numbers = new NativeArray<float>(parameters, Allocator.TempJob);

            this.count = numbers != null ? numbers.Length : 0;

            this.command = command;

            this.result = new NativeArray<float>(1, Allocator.TempJob);
        }

        public void Execute()
        {
            switch (command)
            {
                case Command.Sum:
                    {
                        result[0] = 0;

                        for (int i = 0; i < count; i++)
                        {
                            result[0] += numbers[i];
                        }
                    }
                    break;
                case Command.Min:
                    {
                        result[0] = count > 0 ? numbers[0] : 0;

                        for (int i = 1; i < count; i++)
                        {
                            if (result[0] > numbers[i])
                            {
                                result[0] = numbers[i];
                            }
                        }
                    }
                    break;
                case Command.Max:
                    {
                        result[0] = count > 0 ? numbers[0] : 0;

                        for (int i = 1; i < count; i++)
                        {
                            if (result[0] < numbers[i])
                            {
                                result[0] = numbers[i];
                            }
                        }
                    }
                    break;
                case Command.Count:
                    {
                        result[0] = count;
                    }
                    break;
            }
        }

        public void Dispose()
        {
            result.Dispose();
        }

        public enum Command
        {
            Sum,
            Min,
            Max,
            Count,
        }
    }
}