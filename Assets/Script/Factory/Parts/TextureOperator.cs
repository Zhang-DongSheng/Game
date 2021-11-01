using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    public class TextureOperator : Operator
    {
        public TextureOperator(ResourceInformation resource) : base(resource)
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