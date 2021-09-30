using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    public class GameObjectOperator : Operator
    {
        public GameObjectOperator(ResourceInformation resource) : base(resource)
        {

        }

        public override Object Pop(string value)
        {
            if (memory.Count > 0)
            {
                return memory.Pop();
            }
            else
            {
                return GameObject.Instantiate(asset);
            }
        }

        public override void Push(Object asset)
        {
            if (memory.Count < capacity || capacity == -1)
            {
                memory.Push(asset);
            }
            else
            {
                GameObject.Destroy(asset);
            }
        }
    }
}