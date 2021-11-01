using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    public class AudioOperator : Operator
    {
        public AudioOperator(ResourceInformation resource) : base(resource)
        {

        }

        protected override Object Create()
        {
            return GameObject.Instantiate(asset);
        }

        protected override void Destroy(Object asset)
        {
            GameObject.Destroy(asset);
        }
    }
}