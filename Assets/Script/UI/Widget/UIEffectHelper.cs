using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Transform))]
    public class UIEffectHelper : MonoBehaviour
    {
        [SerializeField] private bool auto = true;

        [SerializeField] private bool ignore = false;

        [SerializeField] private bool variety = false;

        [SerializeField] private Color color = Color.white;

        [SerializeField, Range(0, 255)] private int lighteness = 255;

        private GameObject target;

        private Canvas canvas;

        private SortingGroup[] groups;

        private Renderer[] renderers;

        private ParticleSystem[] particles;

        private Renderer m_renderer;

        private readonly List<Material> m_materials = new List<Material>();

        private bool active = true, ctrlActive = true;

        private void Awake()
        {
            target = gameObject;

            groups = target.GetComponentsInChildren<SortingGroup>();

            renderers = target.GetComponentsInChildren<Renderer>();

            particles = target.GetComponentsInChildren<ParticleSystem>();

            EventManager.Register(EventKey.EffectStatus, Notice);
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            SetLighteness(lighteness);
#endif
        }

        private void Start()
        {
            ctrlActive = ignore || Local.GetValue<bool>(LocalKey.EffectStatus);

            SetOrder();

            SetLighteness(lighteness);

            SetActive(active && ctrlActive);
        }

        public void Refresh()
        {
            SetOrder();
            SetLighteness(lighteness);
        }

        public void ShowOrHide(bool active)
        {
            this.active = active;

            SetActive(this.active && ctrlActive);
        }

        private void Notice(EventMessageArgs args)
        {
            if (ignore) return;

            ctrlActive = args.Get<bool>("status");

            SetActive(this.active && ctrlActive);
        }

        private void GetCanvas(Transform target)
        {
            canvas = null; Transform root = target;

            while (root != null)
            {
                canvas = root.GetComponent<Canvas>();
                if (canvas != null) break;
                root = root.parent;
            }
        }

        private void SetOrder()
        {
            if (!auto) return;

            int order = 0;

            GetCanvas(target.transform);

            if (canvas != null)
            {
                order = canvas.sortingOrder + 1;
            }
            //Set SortingGroup Order
            for (int i = 0; i < groups.Length; i++)
            {
                if (groups[i] != null)
                {
                    groups[i].sortingOrder = order;
                }
            }
            //Set Renderer Order
            for (int i = 0; i < renderers.Length; i++)
            {
                m_renderer = renderers[i];

                if (m_renderer != null)
                {
                    m_renderer.sortingOrder = order;
                }
            }
            //Set ParticleSystem Order
            for (int i = 0; i < particles.Length; i++)
            {
                m_renderer = particles[i].GetComponent<Renderer>();

                if (m_renderer != null)
                {
                    m_renderer.sortingOrder = order;
                }
            }
        }

        private void SetLighteness(int lighteness)
        {
            if (!variety) return;

            this.lighteness = lighteness;

            if (this.lighteness < 1) return;

            if (m_materials.Count == 0)
            {
                if (particles != null && particles.Length > 0)
                {
                    for (int i = 0, j = particles.Length; i < j; i++)
                    {
                        if (particles[i].GetComponent<Renderer>().material != null)
                        {
                            m_materials.Add(particles[i].GetComponent<Renderer>().material);
                        }
                    }
                }

                if (renderers != null && renderers.Length > 0)
                {
                    for (int i = 0, j = renderers.Length; i < j; i++)
                    {
                        if (renderers[i].material != null)
                        {
                            m_materials.Add(renderers[i].material);
                        }
                    }
                }
            }

            float value = (float)lighteness / 255f;

            color = new Color(value, value, value, value);

            for (int i = 0, len = m_materials.Count; i < len; i++)
            {
                if (m_materials[i].HasProperty("_TintColor"))
                {
                    m_materials[i].SetColor("_TintColor", color);
                }
            }
        }

        private void SetActive(bool active)
        {
            if (target != null && target.activeSelf != active)
            {
                target.SetActive(active);
            }
        }

        private void OnDestroy()
        {
            EventManager.Unregister(EventKey.EffectStatus, Notice);
        }
    }
}