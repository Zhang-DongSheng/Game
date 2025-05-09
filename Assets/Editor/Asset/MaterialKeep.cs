using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    [CreateAssetMenu(fileName = "MaterialKeep", menuName = "MaterialKeep", order = 301)]
    public class MaterialKeep : ScriptableObject
    {
        [SerializeField] private List<Material> materials;

        public void Keep()
        {
            foreach (var material in materials)
            {
                switch (material.shader.name)
                {
                    case "UI/Hole":
                        material.SetVector("_Area", Vector4.zero);
                        break;
                    default:
                        Debug.LogWarning($"Material {material.name} shader is {material.shader.name}");
                        break;
                }
                EditorUtility.SetDirty(material);
            }
            AssetDatabase.Refresh();
        }
    }
}