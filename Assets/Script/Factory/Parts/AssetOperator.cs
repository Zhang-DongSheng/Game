using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    public class AssetOperator : Operator
    {
        public AssetOperator(ResourceInformation resource) : base(resource)
        {

        }

        public override Object Pop(string value)
        {
            return asset;
        }

        public override void Push(Object asset)
        {

        }
    }
}