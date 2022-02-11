using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombine : MonoBehaviour
{
    private void Start()
    {
        CombineMesh2();
    }


    private void CombineMesh()
    {
        //获取自身和所有子物体中所有的 MeshRenderer 组件
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        //材质球数组
        Material[] materials = new Material[meshRenderers.Length];

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            materials[i] = meshRenderers[i].sharedMaterial;
        }

        // 合并 Mesh
        // 后去自身和子物体中所有 MsehFilter 组件
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

        CombineInstance[] combines = new CombineInstance[meshRenderers.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combines[i].mesh = meshFilters[i].sharedMesh;
            // 矩阵（Matrix）自身空间坐标的点转换成世界空间坐标的点
            combines[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        //重新生成mesh
        if (!transform.TryGetComponent(out MeshFilter filter))
        {
            filter = gameObject.AddComponent<MeshFilter>();
        }
        filter.mesh = new Mesh();

        // 给 MeshFilter 组件的 mesh 赋值

        //合并Mesh， 第二个参数 false，表示并不合并为一个网格，而是一个自网格列表
        filter.mesh.CombineMeshes(combines, false);
        transform.gameObject.SetActive(true);

        //为合并后的新Mesh 指定材质
        //transform.GetComponent<MeshRenderer>().sharedMaterials = materials;

        if (!transform.TryGetComponent(out MeshRenderer renderer))
        {
            renderer = gameObject.AddComponent<MeshRenderer>();
        }
        renderer.sharedMaterials = materials;
    }

    private void CombineMesh2()
    {
        //获取所有子物体的网格
        MeshFilter[] mfChildren = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[mfChildren.Length];
        //获取所有子物体的渲染器和材质
        MeshRenderer[] mrChildren = GetComponentsInChildren<MeshRenderer>();
        Material[] materials = new Material[mrChildren.Length];
        //生成新的渲染器和网格组件
        MeshRenderer mrSelf = gameObject.AddComponent<MeshRenderer>();
        MeshFilter mfSelf = gameObject.AddComponent<MeshFilter>();
        //合并子纹理
        Texture2D[] textures = new Texture2D[mrChildren.Length];
        for (int i = 0; i < mrChildren.Length; i++)
        {
            if (mrChildren[i].transform == transform)
            {
                continue;
            }
            materials[i] = mrChildren[i].sharedMaterial;
            Texture2D tx = materials[i].GetTexture("_MainTex") as Texture2D;
            Texture2D tx2D = new Texture2D(tx.width, tx.height, TextureFormat.ARGB32, false);
            tx2D.SetPixels(tx.GetPixels(0, 0, tx.width, tx.height));
            tx2D.Apply();
            textures[i] = tx2D;
        }
        //生成新的材质
        Material materialNew = new Material(materials[0].shader);
        materialNew.CopyPropertiesFromMaterial(materials[0]);
        mrSelf.sharedMaterial = materialNew;
        //设置新材质的主纹理
        Texture2D texture = new Texture2D(1024, 1024);
        materialNew.SetTexture("_MainTex", texture);
        Rect[] rects = texture.PackTextures(textures, 10, 1024);
        //根据纹理合并的信息刷新子网格UV
        for (int i = 0; i < mfChildren.Length; i++)
        {
            if (mfChildren[i].transform == transform)
            {
                continue;
            }
            Rect rect = rects[i];
            Mesh meshCombine = mfChildren[i].mesh;
            Vector2[] uvs = new Vector2[meshCombine.uv.Length];
            //把网格的uv根据贴图的rect刷一遍  
            for (int j = 0; j < uvs.Length; j++)
            {
                uvs[j].x = rect.x + meshCombine.uv[j].x * rect.width;
                uvs[j].y = rect.y + meshCombine.uv[j].y * rect.height;
            }
            meshCombine.uv = uvs;
            combine[i].mesh = meshCombine;
            combine[i].transform = mfChildren[i].transform.localToWorldMatrix;
            mfChildren[i].gameObject.SetActive(false);
        }
        //生成新的网格，赋值给新的网格渲染组件
        Mesh newMesh = new Mesh();
        newMesh.CombineMeshes(combine, true, true);//合并网格  
        mfSelf.mesh = newMesh;
    }
}