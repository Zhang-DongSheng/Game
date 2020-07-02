using UnityEngine;
using UnityEngine.Rendering;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
[RequireComponent(typeof(RectTransform))]
public class UIEffectHelper : MonoBehaviour
{
    [Tooltip("自动定位图层")]
    public bool auto = true;
    [Tooltip("忽略特效控制")]
    public bool ignore = false;

    private GameObject target;

    private Canvas canvas;

    private SortingGroup[] groups;

    private Renderer[] renderers;

    private ParticleSystem[] particles;

    private Renderer m_renderer;

    private bool active = true, ctrlActive = true;

    private void Awake()
    {
        target = gameObject;

        groups = target.GetComponentsInChildren<SortingGroup>();

        renderers = target.GetComponentsInChildren<Renderer>();

        particles = target.GetComponentsInChildren<ParticleSystem>();

        EventManager.RegisterEvent(EventKey.EffectStatus, Notice);
    }

    private void Start()
    {
        ctrlActive = ignore ? true : PlayerPrefs.GetInt(PlayerPrefsConfig.EffectStatus).Equals(0);

        SetOrder();

        SetActive(active && ctrlActive);
    }

    public void Refresh()
    {
        SetOrder();
    }

    public void ShowOrHide(bool active)
    {
        this.active = active;

        SetActive(this.active && ctrlActive);
    }

    private void Notice(EventMessageArgs args)
    {
        if (ignore) return;

        ctrlActive = args.GetMessage<bool>("active");

        SetActive(active && ctrlActive);
    }

    private void SetOrder()
    {
        if (!auto) return;

        int order = 1;

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

    private void GetCanvas(Transform target)
    {
        Transform root = target; canvas = null;

        while (root != null)
        {
            canvas = root.GetComponent<Canvas>();
            if (canvas != null) break;
            root = root.parent;
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
        EventManager.UnregisterEvent(EventKey.EffectStatus, Notice);
    }
}