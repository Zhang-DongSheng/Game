 
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace BDSZ_2020
{
    [ExecuteInEditMode]

    
    public class BDSZ_UIParticlesResMgr : MonoBehaviour
    {
        const string c_strFontResManager = "BDSZ_UIParticlesResMgr";
       
        public const string c_MainTexture = "_MainTex";
        const string c_UIEffectShaderName = "UIShader/UIParticles";
        private const int c_iMaxParticleDef = 1024 * 64;
        const int c_iMaxIndexBufferCache = 32;
        protected List<int[]> m_QuadIndicesCache = new List<int[]>(c_iMaxIndexBufferCache);


        public class StMaterialCache
        {

            public Material mat;
            public int iReferCount;
        }
        private static BDSZ_UIParticlesResMgr s_Instance;
        protected List<StMaterialCache>[] m_UIMaterialCaches = new List<StMaterialCache>[(int)EEffectShaderType.Count];

        protected StUIParticleDef[] m_EmitterParticleData;
        protected StUIParticleDef m_EmitterParticleFreeHead;
       
        public static BDSZ_UIParticlesResMgr Instance
        {
            get
            {
                GameObject go = GameObject.Find(c_strFontResManager);
                if (go == null)
                {
                    go = new GameObject(c_strFontResManager);
#if !UNITY_EDITOR
                    GameObject.DontDestroyOnLoad(go);
#endif
                }
                s_Instance = go.GetComponent<BDSZ_UIParticlesResMgr>() as BDSZ_UIParticlesResMgr;
                if (s_Instance == null)
                    s_Instance = go.AddComponent<BDSZ_UIParticlesResMgr>();
                s_Instance.Init();
                return s_Instance;
            }
        }
        public void Init()
        {
            
            for(int i=0;i<m_UIMaterialCaches.Length;i++)
            {
                m_UIMaterialCaches[i] = new List<StMaterialCache>();
            }
 
        }
        public int[] GetQuadrangleIndices(int iQuadrangleCount)
        {

            for (int i = 0, imax = m_QuadIndicesCache.Count; i < imax; ++i)
            {
                int[] ids = m_QuadIndicesCache[i];
                if (ids != null && ids.Length == 6 * iQuadrangleCount)
                {
                    return ids;
                }
            }

            int[] iIndices = new int[6 * iQuadrangleCount];
            int iVertexIndex = 0;
            for (int i = 0; i < iQuadrangleCount * 6; i += 6)
            {
                iIndices[i + 0] = iVertexIndex + 0;
                iIndices[i + 1] = iVertexIndex + 2;
                iIndices[i + 2] = iVertexIndex + 1;

                iIndices[i + 3] = iVertexIndex + 0;
                iIndices[i + 4] = iVertexIndex + 3;
                iIndices[i + 5] = iVertexIndex + 2;

                iVertexIndex += 4;
            }
            if (m_QuadIndicesCache.Count >= c_iMaxIndexBufferCache)
                m_QuadIndicesCache.RemoveAt(0);
            m_QuadIndicesCache.Add(iIndices);
            return iIndices;
        }

        void MallocParticleDef()
        {
            m_EmitterParticleData = new StUIParticleDef[c_iMaxParticleDef];
            for (int i = 0; i < c_iMaxParticleDef; i++)
            {
                StUIParticleDef p = new StUIParticleDef();
                m_EmitterParticleData[i] = p;
                p.Next = m_EmitterParticleFreeHead;
                m_EmitterParticleFreeHead = p;
            }
        }
        public StUIParticleDef GetFreeParticle()
        {
            if (m_EmitterParticleData == null)
                MallocParticleDef();
            if (m_EmitterParticleFreeHead == null)
                return null;
            StUIParticleDef pResult = m_EmitterParticleFreeHead;
            m_EmitterParticleFreeHead = pResult.Next;
            pResult.Next = null;
            return pResult;
        }
        public void FreeParticle(StUIParticleDef p)
        {
            p.Next = m_EmitterParticleFreeHead;
            m_EmitterParticleFreeHead = p;
        }

       

        void InnerRelease()
        {
            for (int i = 0; i < m_UIMaterialCaches.Length; i++)
            {
                for (int im = 0; im < m_UIMaterialCaches[i].Count; im++)
                {
                    GameObject.DestroyImmediate(m_UIMaterialCaches[i][im].mat);
                }
                m_UIMaterialCaches[i].Clear();
            }


        }
      
        static public void Destroy()
        {
            if (s_Instance != null)
            {
                s_Instance.InnerRelease();
                s_Instance = null;
            }

            GC.Collect();
        }
        void InnerReleaseMaterial(Material mat, int iTypeIndex)
        {
            for (int i = 0; i < m_UIMaterialCaches[iTypeIndex].Count; i++)
            {
                if (m_UIMaterialCaches[iTypeIndex][i].mat != mat)
                    continue;
                if (--m_UIMaterialCaches[iTypeIndex][i].iReferCount == 0)
                {
                    DestroyImmediate(m_UIMaterialCaches[iTypeIndex][i].mat);
                    m_UIMaterialCaches[iTypeIndex].RemoveAt(i);
                }
            }
        }
        static public void ReleaseMaterial(Material mat, int iTypeIndex)
        {

            if (s_Instance != null)
            {
                s_Instance.InnerReleaseMaterial(mat, iTypeIndex);
            }
        }
     
       
        public void ApplayShaderType(Material material, EEffectShaderType blendMode)
        {

            switch (blendMode)
            {
                case EEffectShaderType.Transparent:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    break;
                case EEffectShaderType.Additive:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    break;
                case EEffectShaderType.SoftAdditive:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusDstColor);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    break;
                case EEffectShaderType.ColorAdditive:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcColor);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcColor);
                    break;
               
            }
        }
   
        
      


        public Material CreateMaterial(EEffectShaderType type, Texture texture)
        {
            int iTypeIndex = (int)type;
            for (int i = 0; i < m_UIMaterialCaches[iTypeIndex].Count; i++)
            {
                if (m_UIMaterialCaches[iTypeIndex][i].mat == null)
                    continue;

                if (m_UIMaterialCaches[iTypeIndex][i].mat.mainTexture == texture)
                {
                    m_UIMaterialCaches[iTypeIndex][i].iReferCount++;
                    return m_UIMaterialCaches[iTypeIndex][i].mat;
                }

            }
            StMaterialCache newCache = new StMaterialCache();
            Shader shader = Shader.Find(c_UIEffectShaderName);
            Material mat = new Material(shader)
            {
                hideFlags =  HideFlags.DontSave | HideFlags.NotEditable | HideFlags.HideInInspector,
                mainTexture = texture
            };
            newCache.mat = mat;
            ApplayShaderType(mat, type);
            newCache.iReferCount++;
            m_UIMaterialCaches[iTypeIndex].Add(newCache);
            return newCache.mat;
        }
#if UNITY_EDITOR
        static string s_strSelectBoundsName = "SelectBounds";
        static Material s_SelectMat = null;
        static BDSZ_CanvasDraw s_PublicCanvasRenderer = null;
        static CanvasRenderer s_UISelectBoundRender = null;

        static public BDSZ_CanvasDraw PublicCanvasRenderer
        {
            get
            {
                if (s_PublicCanvasRenderer == null)
                {
                    s_PublicCanvasRenderer = new BDSZ_CanvasDraw();

                }
                return s_PublicCanvasRenderer;
            }
        }
        static public BDSZ_CanvasDraw GetSelectBoundRenderer(GameObject parentNode)
        {
            if (s_UISelectBoundRender == null)
            {
                Transform childNode = parentNode.transform.Find(s_strSelectBoundsName);
                if (childNode != null)
                {
                    s_UISelectBoundRender = childNode.GetComponent<CanvasRenderer>();
                }
                else
                {
                    GameObject child =BDSZ_UIParticlesUtilities.CreateChildGo(parentNode, s_strSelectBoundsName,  HideFlags.HideInHierarchy);
                    s_UISelectBoundRender = child.AddComponent<CanvasRenderer>();
                }

               

            }
            if(s_SelectMat==null)
            {
                s_SelectMat = new Material(Shader.Find("UI/Default"))
                {
                    mainTexture = Texture2D.whiteTexture
                };
                s_UISelectBoundRender.SetMaterial(s_SelectMat, null);
            }

            s_UISelectBoundRender.transform.position = parentNode.transform.position;
            s_UISelectBoundRender.transform.localPosition = Vector3.zero;
            s_UISelectBoundRender.transform.eulerAngles = Vector3.zero;
            s_UISelectBoundRender.transform.localScale = Vector3.one;
            s_UISelectBoundRender.transform.SetParent(parentNode.transform, false);
            PublicCanvasRenderer.CanvasRender = s_UISelectBoundRender;
            PublicCanvasRenderer.BeginDraw();
            return PublicCanvasRenderer;

        }
        static public bool IsSelected(GameObject go)
        {
            GameObject compareGo = Selection.activeGameObject;
            while (compareGo != null)
            {
                if (go == compareGo)
                    return true;
                if (compareGo.transform.parent == null)
                    break;
                compareGo = compareGo.transform.parent.gameObject;
            }
            return false;

        }
        static public void Prepare()
        {
            if(s_Instance==null)
            {
                BDSZ_UIParticlesResMgr.Instance.Init();
            }

        }
        

#endif


    }


}