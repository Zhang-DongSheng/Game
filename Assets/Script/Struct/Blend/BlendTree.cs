using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BlendTree : MonoBehaviour
    {
        public float value;

        public List<BlendNode> nodes;

        public BlendData Blend()
        {
            int count = nodes.Count;

            switch (count)
            {
                case 0:
                    {
                        return null;
                    }
                case 1:
                    {
                        return nodes[0].Blend();
                    }
                default:
                    {
                        if (value >= nodes[count - 1].value)
                        {
                            return nodes[count - 1].Blend();
                        }
                        else
                        {
                            for (int i = 0; i < count - 1; i++)
                            {
                                if (value >= nodes[i].value && value <= nodes[i + 1].value)
                                {
                                    return nodes[i].Blend().Lerp(nodes[i + 1].Blend(), Progress(value, nodes[i].value, nodes[i + 1].value));
                                }
                            }
                            return null;
                        }
                    }
            }
        }

        public float Progress(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }
    }
}