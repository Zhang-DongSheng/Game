using Game;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor
{
    public class PrefabUtils
    {
        public static GameObject CreateUGUI(string path)
        {
            string extension = Path.GetExtension(path);

            string name = Path.GetFileNameWithoutExtension(path);

            GameObject entity = new GameObject(name, typeof(RectTransform));

            entity.GetComponent<RectTransform>().SetFull();

            var child = new GameObject("Background", typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image));
            child.transform.SetParent(entity.transform, false);

            child.GetComponent<RectTransform>().SetFull();

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(entity, path, out bool success);

            GameObject.DestroyImmediate(entity);

            PrefabUtility.SavePrefabAsset(prefab);

            return prefab;
        }
    }
}