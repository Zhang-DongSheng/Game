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
        //��ȡ��������������������е� MeshRenderer ���
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        //����������
        Material[] materials = new Material[meshRenderers.Length];

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            materials[i] = meshRenderers[i].sharedMaterial;
        }

        // �ϲ� Mesh
        // ��ȥ����������������� MsehFilter ���
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

        CombineInstance[] combines = new CombineInstance[meshRenderers.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combines[i].mesh = meshFilters[i].sharedMesh;
            // ����Matrix������ռ�����ĵ�ת��������ռ�����ĵ�
            combines[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        //��������mesh
        if (!transform.TryGetComponent(out MeshFilter filter))
        {
            filter = gameObject.AddComponent<MeshFilter>();
        }
        filter.mesh = new Mesh();

        // �� MeshFilter ����� mesh ��ֵ

        //�ϲ�Mesh�� �ڶ������� false����ʾ�����ϲ�Ϊһ�����񣬶���һ���������б�
        filter.mesh.CombineMeshes(combines, false);
        transform.gameObject.SetActive(true);

        //Ϊ�ϲ������Mesh ָ������
        //transform.GetComponent<MeshRenderer>().sharedMaterials = materials;

        if (!transform.TryGetComponent(out MeshRenderer renderer))
        {
            renderer = gameObject.AddComponent<MeshRenderer>();
        }
        renderer.sharedMaterials = materials;
    }

    private void CombineMesh2()
    {
        //��ȡ���������������
        MeshFilter[] mfChildren = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[mfChildren.Length];
        //��ȡ�������������Ⱦ���Ͳ���
        MeshRenderer[] mrChildren = GetComponentsInChildren<MeshRenderer>();
        Material[] materials = new Material[mrChildren.Length];
        //�����µ���Ⱦ�����������
        MeshRenderer mrSelf = gameObject.AddComponent<MeshRenderer>();
        MeshFilter mfSelf = gameObject.AddComponent<MeshFilter>();
        //�ϲ�������
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
        //�����µĲ���
        Material materialNew = new Material(materials[0].shader);
        materialNew.CopyPropertiesFromMaterial(materials[0]);
        mrSelf.sharedMaterial = materialNew;
        //�����²��ʵ�������
        Texture2D texture = new Texture2D(1024, 1024);
        materialNew.SetTexture("_MainTex", texture);
        Rect[] rects = texture.PackTextures(textures, 10, 1024);
        //��������ϲ�����Ϣˢ��������UV
        for (int i = 0; i < mfChildren.Length; i++)
        {
            if (mfChildren[i].transform == transform)
            {
                continue;
            }
            Rect rect = rects[i];
            Mesh meshCombine = mfChildren[i].mesh;
            Vector2[] uvs = new Vector2[meshCombine.uv.Length];
            //�������uv������ͼ��rectˢһ��  
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
        //�����µ����񣬸�ֵ���µ�������Ⱦ���
        Mesh newMesh = new Mesh();
        newMesh.CombineMeshes(combine, true, true);//�ϲ�����  
        mfSelf.mesh = newMesh;
    }
}