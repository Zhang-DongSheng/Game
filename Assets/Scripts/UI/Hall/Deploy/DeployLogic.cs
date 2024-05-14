using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class DeployLogic : MonoSingleton<DeployLogic>
    {
        [UnityEngine.SerializeField] private List<Transform> pedestals;

        public void Trigger(int index)
        {
            int count = pedestals.Count;

            for(int i = 0;i< count;i++)
            {
                var material = pedestals[i].GetComponent<Renderer>().material;

                if (index == i)
                {
                    material.color = Color.red;
                }
                else
                {
                    material.color = Color.green;
                }
            }
        }

        public int GetIndex(Transform target)
        { 
            return pedestals.IndexOf(target);
        }
    }
}