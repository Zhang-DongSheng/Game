using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game
{
    public class ModelRaycast : MonoBehaviour
    {
        [SerializeField] private LayerMask layer;

        [SerializeField] private float distance = 1000f;

        [SerializeField] private new Camera camera;

        [Space(5)] public UnityEvent<RaycastHit> onClick;

        private int layerMask;

        private Ray ray;

        private RaycastHit hit;

        private void Awake()
        {
            if (camera == null)
            {
                camera = Camera.main;
            }
            layerMask = LayerMask.GetMask(layer.ToString());
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventInterception)
                {
                    ray = camera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit, distance, layerMask))
                    {
                        onClick?.Invoke(hit);
                    }
                }
            }
        }

        private bool EventInterception
        {
            get
            {
                if (EventSystem.current == null) return true;
#if UNITY_EDITOR
                if (EventSystem.current.IsPointerOverGameObject())
#else
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#endif
                {
                    return true;
                }
                return false;
            }
        }
    }
}