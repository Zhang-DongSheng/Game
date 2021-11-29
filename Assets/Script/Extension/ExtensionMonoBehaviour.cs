using System.Collections;
using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        public static void InvokeSafe(this MonoBehaviour behavior, System.Action method, float delayInSeconds)
        {
            behavior.StartCoroutine(InvokeSafeRoutine(method, delayInSeconds));
        }

        public static void InvokeRepeatingSafe(this MonoBehaviour behavior, System.Action method, float delayInSeconds, float repeatRateInSeconds)
        {
            behavior.StartCoroutine(InvokeSafeRepeatingRoutine(method, delayInSeconds, repeatRateInSeconds));
        }

        internal static IEnumerator InvokeSafeRepeatingRoutine(System.Action method, float delayInSeconds, float repeatRateInSeconds)
        {
            yield return new WaitForSeconds(delayInSeconds);

            while (true)
            {
                if (method != null) method.Invoke();
                yield return new WaitForSeconds(repeatRateInSeconds);
            }
        }

        internal static IEnumerator InvokeSafeRoutine(System.Action method, float delayInSeconds)
        {
            yield return new WaitForSeconds(delayInSeconds);
            if (method != null) method.Invoke();
        }
    }
}