using UnityEngine;

namespace Game.Model
{
    public class ModelDisplayCell : MonoBehaviour
    {
        public int serial;

        [SerializeField] private GameObject empty;

        private ModelDisplay m_display;

        public void Refresh(ModelDisplayInformation model)
        {
            if (m_display == null)
            {
                m_display = new GameObject("display").AddComponent<ModelDisplay>();

                m_display.transform.SetParent(transform, false);

                m_display.transform.localPosition = Vector3.zero;
            }
            m_display.Refresh(model);
        }

        public void Release()
        {
            if (m_display != null)
            { 
                m_display.Release();
            }
        }

        public void Empty(bool active)
        {
            if (empty != null && empty.activeSelf != active)
            {
                empty.SetActive(active);
            }
        }

        public ModelDisplay Display => m_display;
    }
}