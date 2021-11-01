using Data;
using UnityEngine.U2D;

namespace UnityEngine
{
    public class AtlasOperator : Operator
    {
        public AtlasOperator(ResourceInformation resource) : base(resource)
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