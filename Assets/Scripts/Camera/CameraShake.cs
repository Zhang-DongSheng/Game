using System.Collections;
using UnityEngine;

namespace Game
{
    public class CameraShake : ItemBase
    {
        [SerializeField] private ShakePattern shake;

        [SerializeField] private float intensityMin = 0.1f;

        [SerializeField] private float intensityMax = 0.5f;

        [SerializeField] private float decay = 0.02f;

        private Vector3 position;

        private Quaternion rotation;

        private Coroutine coroutine;

        private void OnDisable()
        {
            coroutine = null;
        }

        public void Shake()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(ProcessShake());
        }

        IEnumerator ProcessShake()
        {
            position = transform.position;

            rotation = transform.rotation;

            float intensity = Random.Range(intensityMin, intensityMax);

            while (intensity > 0)
            {
                switch (shake)
                {
                    case ShakePattern.Translate:
                        transform.position = position + Random.insideUnitSphere * intensity;
                        break;
                    case ShakePattern.Rotate:
                        {
                            transform.rotation = new Quaternion(rotation.x + Random.Range(-intensity, intensity) * Time.deltaTime,
                                                                rotation.y + Random.Range(-intensity, intensity) * Time.deltaTime,
                                                                rotation.z + Random.Range(-intensity, intensity) * Time.deltaTime,
                                                                rotation.w + Random.Range(-intensity, intensity) * Time.deltaTime);
                        }
                        break;
                    case ShakePattern.TranslateAndRotate:
                        {
                            transform.position = position + Random.insideUnitSphere * intensity;
                            goto case ShakePattern.Rotate;
                        }
                }
                intensity -= decay * Time.deltaTime;

                yield return null;
            }
            OnCompleted();
        }

        private void OnCompleted()
        {
            transform.position = position;

            transform.rotation = rotation;

            coroutine = null;
        }
        [ContextMenu("Debug")]
        protected void Debug()
        {
            Shake();
        }
    }

    enum ShakePattern
    { 
        None,
        Translate,
        Rotate,
        TranslateAndRotate,
    }
}