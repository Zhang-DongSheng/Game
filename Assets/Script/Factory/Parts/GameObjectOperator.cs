using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine
{
    public class GameObjectOperator : Operator
    {
        public GameObjectOperator(ResourceInformation resource) : base(resource)
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