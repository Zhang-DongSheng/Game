using UnityEngine;

namespace Game.Develop
{
    [ExecuteInEditMode]
    public sealed class DevelopController : MonoSingleton<DevelopController>
    {
        [SerializeField] private DevelopView m_view;

        private void Awake()
        {
            if (m_view == null)
            {
                GameObject target = new GameObject("view");

                target.transform.parent = transform;

                m_view = target.AddComponent<DevelopView>();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                DisplayDevelop();
            }
        }

        private void DisplayDevelop()
        {
            if (m_view != null)
            {
                m_view.Open();
            }
        }
    }
}