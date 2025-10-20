using Game.Attribute;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Graphic))]
    public class UIDissolve : MonoBehaviour
    {
        [SerializeField] private Material source;

        [SerializeField] private List<Graphic> graphics;
        [Button("RelevanceGraphics", "自动关联")]
        [SerializeField] private bool relevance;

        private Material material;

        private void Awake()
        {
            material = new Material(source);

            for (int i = 0; i < graphics.Count; i++)
            {
                graphics[i].material = material;
            }
        }

        private void Update()
        {
            //material
        }

        private void RelevanceGraphics()
        {
            graphics.Clear();

            graphics.AddRange(GetComponentsInChildren<Graphic>());
        }
    }
}