using UnityEngine;

namespace Game.UI
{
    public class UIWaiting : UIBase
    {
        [SerializeField] private Transform target;

        [SerializeField, Range(0, 100f)] private float angle = 1f;

        private Vector3 vector;

        protected override void OnAwake()
        {
            vector = Vector3.forward * angle;
        }

        protected override void OnUpdate(float delta)
        {
            target.Rotate(vector * Time.deltaTime);
        }
    }
}