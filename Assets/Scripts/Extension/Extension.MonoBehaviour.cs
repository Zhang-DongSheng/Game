using System.Collections;
using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        public static void InvokeSafe(this MonoBehaviour behavior, System.Action method, float delayInSeconds)
        {
            YieldInstruction wait = null;

            if (delayInSeconds > 0)
            {
                wait = new WaitForSeconds(delayInSeconds);
            }
            behavior.StartCoroutine(InvokeSafeRoutine(method, wait));
        }

        public static void InvokeRepeatingSafe(this MonoBehaviour behavior, System.Action method, float delayInSeconds, float repeatRateInSeconds)
        {
            YieldInstruction wait = null;

            if (delayInSeconds > 0)
            {
                wait = new WaitForSeconds(delayInSeconds);
            }
            var repeat = new WaitForSeconds(repeatRateInSeconds);

            behavior.StartCoroutine(InvokeSafeRepeatingRoutine(method, wait, repeat));
        }

        internal static IEnumerator InvokeSafeRoutine(System.Action method, YieldInstruction wait)
        {
            yield return wait;

            method?.Invoke();
        }

        internal static IEnumerator InvokeSafeRepeatingRoutine(System.Action method, YieldInstruction wait, YieldInstruction repeat)
        {
            yield return wait;

            while (true)
            {
                method?.Invoke();

                yield return repeat;
            }
        }
    }
}