using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class BlendTree<T> : MonoBehaviour where T : BlendBase, new()
    {
        [SerializeField] protected float threshold;

        [SerializeField] protected List<T> nodes = new List<T>();

        [SerializeField] protected T blend = new T();

        private int count;

        private void OnValidate()
        {
            Blend();
        }

        public virtual BlendBase Blend()
        {
            count = nodes.Count;

            switch (count)
            {
                case 0:
                    {
                        return blend;
                    }
                case 1:
                    {
                        return blend.Copy(nodes[0]);
                    }
                default:
                    {
                        if (threshold <= nodes[0].threshold)
                        {
                            return blend.Copy(nodes[0]);
                        }
                        else if (threshold >= nodes[count - 1].threshold)
                        {
                            return blend.Copy(nodes[count - 1]);
                        }
                        else
                        {
                            for (int i = 0; i < count - 1; i++)
                            {
                                if (threshold >= nodes[i].threshold && threshold <= nodes[i + 1].threshold)
                                {
                                    blend.Lerp(nodes[i], nodes[i + 1], Progress(threshold, nodes[i].threshold, nodes[i + 1].threshold));
                                    break;
                                }
                            }
                        }
                    }
                    return blend;
            }
        }

        private float Progress(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }

        public float Threshold
        {
            get { return threshold; }
            set
            {
                threshold = value;
            }
        }
    }

    public abstract class BlendBase
    {
        public float threshold;

        public abstract BlendBase Lerp(BlendBase from, BlendBase to, float progress);

        public abstract BlendBase Copy(BlendBase target);
    }
}