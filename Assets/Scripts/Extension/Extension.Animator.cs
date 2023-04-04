using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        public static float GetClipTime(this Animator animator, string name)
        {
            if (animator == null) return -1;

            var controller = animator.runtimeAnimatorController;

            var clips = controller.animationClips;

            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i] == null) continue;

                if (clips[i].name == name)
                {
                    return clips[i].length;
                }
            }
            return -1;
        }

        public static float GetClipTimeByLayer(this Animator animator, string name, int layer = 0)
        {
            AnimatorClipInfo[] clips = animator.GetCurrentAnimatorClipInfo(layer);

            int count = clips.Length;

            for (int i = 0; i < count; i++)
            {
                if (clips[i].clip.name == name)
                {
                    return clips[i].clip.length * animator.GetNormalizedTime(true);
                }
            }
            return -1;
        }

        public static int GetClipFrame(this Animator animator, string name, int layer = 0)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(layer);

            AnimatorClipInfo[] clips = animator.GetCurrentAnimatorClipInfo(layer);

            AnimationClip clip = null;

            int count = clips.Length;

            for (int i = 0; i < count; i++)
            {
                if (clips[i].clip.name == name)
                {
                    clip = clips[i].clip;
                    break;
                }
            }
            if (clip == null) return -1;

            return Mathf.RoundToInt(clip.frameRate * state.length * animator.GetNormalizedTime(true));
        }

        public static float GetNormalizedTime(this Animator animator, bool clamp, int layer = 0)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(layer);

            if (clamp)
            {
                return state.normalizedTime % 1f;
            }
            else
            {
                return state.normalizedTime;
            }
        }
    }
}