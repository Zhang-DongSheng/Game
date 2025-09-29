using UnityEngine;

namespace Game.Data
{
    public static class Common
    {
        public static readonly YieldInstruction WaitEndFrame = new WaitForEndOfFrame();

        public static readonly YieldInstruction Wait1s = new WaitForSeconds(1);

        public static readonly YieldInstruction Wait3s = new WaitForSeconds(3);
    }
}