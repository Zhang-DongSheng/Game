using UnityEngine;

namespace Game.UI
{
    public class UIWaiting : MonoBehaviour
    {
        [SerializeField] private Transform icon;

        [SerializeField, Range(0, 100f)] private float angle = 1f;

        [SerializeField] private bool forward;

        private Vector3 vector;

        private void Awake()
        {
            float ratio = forward ? angle : angle * -1f;

            vector = Vector3.forward * ratio;
        }

        private void OnValidate()
        {
            float ratio = forward ? angle : angle * -1f;

            vector = Vector3.forward * ratio;
        }

        private void Update()
        {
            icon.Rotate(vector * Time.deltaTime);
        }

        public void SetActive(bool active)
        {
            if (gameObject != null && gameObject.activeSelf != active)
            {
                gameObject.SetActive(active);
            }
        }
    }
}