using Data;
using UnityEngine.U2D;

namespace UnityEngine
{
    public class AtlasOperator : Operator
    {
        private SpriteAtlas atlas;

        public AtlasOperator(ResourceInformation resource) : base(resource)
        {
            atlas = asset as SpriteAtlas;
        }

        public override Object Pop(string value)
        {
            return atlas.GetSprite(value);
        }

        public override void Push(Object asset)
        {

        }
    }
}