using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game
{
    public class RaycastClick : MonoBehaviour
    {
        [SerializeField] private LayerMask layer;

        [SerializeField] private float distance = 1000f;

        [SerializeField] private new Camera camera;

        [Space(5)] public UnityEvent<RaycastHit> onClick;

        private Ray ray;

        private RaycastHit hit;

        private void Awake()
        {
            if (camera == null)
            {
                camera = Camera.main;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventInterception)
                {
                    ray = camera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit, distance, layer))
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
                bool over = false;

                if (EventSystem.current != null)
                {
#if UNITY_EDITOR
                    if (EventSystem.current.IsPointerOverGameObject())
                    {
                        over = true;
                    }
#else
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                        {
                            over = true;
                        }
                        else
                        {
                            over = false; break;
                        }
                    }
#endif
                }
                return over;
            }
        }
    }
}