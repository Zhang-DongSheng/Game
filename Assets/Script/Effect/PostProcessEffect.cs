using UnityEngine;

namespace Game.Effect
{
    public class PostProcessEffect : PostProcessing
    {
        [SerializeField] private PostEffect effect;

        protected override void OnValidate()
        {
            Reload(PostConfig.LoadShader(effect));
        }
    }
}