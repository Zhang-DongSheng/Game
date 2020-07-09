using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// 【UI粒子特效显示区域】
    /// 常用在Scroll Rect下，拖动到指定区域才显示
    /// 需要在Shader中指定显示区域，例如：UIEffect.shader
    /// </summary>
    public class UIEffectClip : MonoBehaviour
    {
        [SerializeField] private RectTransform m_rect;                      //遮挡容器，即ScrollView

        [SerializeField] private Vector2 offset;

        [SerializeField] private bool auto = true;

        private bool once = true;

        private Vector4 area;

        private readonly List<Material> m_materials = new List<Material>();

        private void Awake()
        {
            Compute();
        }

        private void Init()
        {
            Vector2 position = m_rect.localPosition;

            if (auto) ComputeOffset();

            float width = m_rect.rect.width * 0.5f * 1;

            float height = m_rect.rect.height * 0.5f * 1;

            area = new Vector4()
            {
                x = position.x - width + offset.x,
                y = position.y - height + offset.y,
                z = position.x + width + offset.x,
                w = position.y + height + offset.y
            };

            once = false;
        }

        private void ComputeOffset()
        {
            Transform node = m_rect;

            offset = Vector2.zero;

            while (node != null)
            {
                offset += (Vector2)node.localPosition;

                node = node.parent;

                if (node == null || node.GetComponent<Canvas>() != null)
                {
                    break;
                }
            }

            auto = false;
        }

        public void Compute()
        {
            if (once) Init();

            m_materials.Clear();

            var particleSystems = GetComponentsInChildren<ParticleSystem>(true);
            for (int i = 0, j = particleSystems.Length; i < j; i++)
            {
                var mat = particleSystems[i].GetComponent<Renderer>().material;
                m_materials.Add(mat);
            }

            var renders = GetComponentsInChildren<MeshRenderer>(true);
            for (int i = 0, j = renders.Length; i < j; i++)
            {
                var mat = renders[i].material;
                m_materials.Add(mat);
            }

            for (int i = 0, len = m_materials.Count; i < len; i++)
            {
                m_materials[i].SetVector("_Area", area);
            }
        }
    }
}