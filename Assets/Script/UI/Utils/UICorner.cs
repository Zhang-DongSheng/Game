using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Graphic)), ExecuteInEditMode]
    public class UICorner : MonoBehaviour
    {
        private const string SHADER = "UI/Corner";

        [SerializeField] private Vector2 leftTop = new Vector2();

        [SerializeField] private Vector2 rightTop = new Vector2();

        [SerializeField] private Vector2 leftBottom = new Vector2();

        [SerializeField] private Vector2 rightBottom = new Vector2();

        private Material m_material;

        private void Awake()
        {
            Material();
        }

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                UpdateMaterial();
            }
        }

        private void UpdateMaterial()
        {
            if (m_material != null)
            { 
                //m_material.SetVector("",)
            }
        }

        private void Material()
        {
            if (m_material == null)
            {
                m_material = GetComponent<Graphic>().material;
            }

            if (m_material != null && m_material.shader != null && m_material.shader.name == SHADER)
            {

            }
            else
            {
                Shader shader = Shader.Find(SHADER);

                m_material = new UnityEngine.Material(shader);

                GetComponent<Graphic>().material = m_material;
            }
        }
    }
}
