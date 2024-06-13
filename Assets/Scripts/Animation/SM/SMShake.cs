using UnityEngine;

namespace Game.SM
{
    /// <summary>
    /// 抖动
    /// </summary>
    public class SMShake : SMBase
    {
        [SerializeField] private float intensity = 0.3f;

        private Quaternion rotation;

        private Vector3 position;

        private float range;

        protected override void Initialize()
        {
            position = target.localPosition;

            rotation = target.localRotation;
        }

        protected override void Transition(float progress)
        {
            if (target == null) return;

            range = Mathf.Lerp(0, intensity, progress);

            target.localPosition = position + Random.insideUnitSphere * range;

            target.localRotation = new Quaternion(
                rotation.x + Random.Range(-range, range) * 0.2f,
                rotation.y + Random.Range(-range, range) * 0.2f,
                rotation.z + Random.Range(-range, range) * 0.2f,
                rotation.w + Random.Range(-range, range) * 0.2f);
        }

        protected override void Completed()
        {
            status = Status.Completed;

            target.localPosition = position;

            target.localRotation = rotation;

            onCompleted?.Invoke();

            status = Status.Idel;
        }
    }
}