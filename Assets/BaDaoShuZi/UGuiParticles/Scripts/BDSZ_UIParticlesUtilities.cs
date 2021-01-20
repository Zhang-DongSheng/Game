using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
namespace BDSZ_2020
{ //ui中对齐方式。
    
    public static class BDSZ_UIParticlesUtilities
    {


        public const string c_strCanvasRender = "__CanvasRender";
        public static bool CompareColor(Color32 a, Color32 b)
        {
            if (a.a == b.a && a.r == b.r && a.g == b.g && a.b == b.b)
                return true;
            return false;
        }
        public static Color32 MultiplyColor(Color32 a, Color32 b)
        {
            Color32 r = a;
            r.a = (byte)((int)a.a * b.a / 255);
            r.r = (byte)((int)a.r * b.r / 255);
            r.g = (byte)((int)a.g * b.g / 255);
            r.b = (byte)((int)a.b * b.b / 255);
            return r;
        }
      
        static private void AssignPositioningIfNeeded(RectTransform childNode, GameObject parentGo)
        {
            childNode.position = Vector3.zero;
            childNode.localPosition = Vector3.zero;
            childNode.eulerAngles = Vector3.zero;
            childNode.localScale = Vector3.one;
        }

        static public GameObject CreateChildGo(GameObject parent, string name, HideFlags hideFlags)
        {

            GameObject go = new GameObject(name,typeof(RectTransform));
            go.transform.SetParent(parent.transform);
            go.layer = parent.layer;
            go.hideFlags = hideFlags;
            AssignPositioningIfNeeded(go.GetComponent<RectTransform>(),parent);
            return go;
        }
        static public int FindBestRenderPosition(Transform tf)
        {
            for (int ic = tf.childCount - 1; ic >= 0; ic--)
            {
                Transform tc = tf.GetChild(ic);
                if (string.Compare(tc.name, 0, c_strCanvasRender, 0, c_strCanvasRender.Length) == 0)
                    return ic + 1;
            }
            return 0;
        }
        static public int GetRelativeDepthFromName(Transform tf)
        {
            int iOrder = 0;
            int iBase = 1;
            if (string.Compare(tf.name, 0, c_strCanvasRender, 0, c_strCanvasRender.Length) != 0)
                return 0;

            for (int i = tf.name.Length - 1; i > 0; i--)
            {
                if (tf.name[i] >= '0' && tf.name[i] <= '9')
                {
                    iOrder = (tf.name[i] - '0') * iBase + iOrder;
                    iBase *= 10;
                }
                else
                    break;
            }
            return iOrder;
        }
     
    }
}