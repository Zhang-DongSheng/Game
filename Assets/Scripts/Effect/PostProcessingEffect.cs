using UnityEngine;

namespace Game.Effect
{
    public class PostProcessingEffect : PostProcessing
    {
        [SerializeField] private PostEffect effect;

        protected override void OnValidate()
        {
            Reload(PostProcessingConfig.LoadShader(effect));
        }
    }
}