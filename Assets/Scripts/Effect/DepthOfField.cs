using UnityEngine;

[ExecuteInEditMode]
public class DepthOfField : MonoBehaviour
{
    public bool ShowGizmosWire = true;
    public Transform FocusOn;
    //归一化后的焦距
    [Tooltip("景深相关：聚焦点的距离，值0-1对应相机远近裁剪平面")]
    [Range(-0.2f, 1.2f)]
    public float FocusDistance = 0.5f;
    [Tooltip("景深相关：聚焦范围")]
    [Range(0, 1f)]
    public float FocusRange = 0.5f;
    [Tooltip("模糊相关：模糊扩散程度")]
    [Range(0.2f, 3.0f)]
    public float BlurSpread = 0.6f;
    [Tooltip("模糊相关：纹理采样遍历次数，值越大性能消耗越大")]
    [Range(0, 4)]
    public int BlurIterations = 3;
    [Tooltip("模糊相关：纹理采样分辨率，值越小性能消耗越大")]
    [Range(1, 8)]
    public int BlurDownSample = 2;
    private Material dMaterial;

    private new Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;

        dMaterial = new Material(Shader.Find("Custom/DepthOfFiled"));
    }

    private void Update()
    {
        if (this.FocusOn != null)
        {
            this.FocusDistance = (this.FocusOn.position.z - this.camera.transform.position.z - camera.nearClipPlane) / (camera.farClipPlane - camera.nearClipPlane);
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        dMaterial.SetFloat("_FocusDistance", FocusDistance);
        dMaterial.SetFloat("_FocusRange", 10f - 10f * FocusRange);
        if (dMaterial != null)
        {
            int rtW = src.width / BlurDownSample;
            int rtH = src.height / BlurDownSample;

            RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
            buffer0.filterMode = FilterMode.Bilinear;

            Graphics.Blit(src, buffer0);

            for (int i = 0; i < BlurIterations; i++)
            {
                dMaterial.SetFloat("_BlurSize", 1.0f + i * BlurSpread);
                RenderTexture buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
                Graphics.Blit(buffer0, buffer1, dMaterial, 0);
                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;
                buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
                Graphics.Blit(buffer0, buffer1, dMaterial, 1);

                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;
            }
            dMaterial.SetTexture("_BlurTex", buffer0);
            Graphics.Blit(src, dest, dMaterial, 2);
            RenderTexture.ReleaseTemporary(buffer0);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (camera == null)
        {
            camera = GetComponent<Camera>();
        }
        if (enabled && ShowGizmosWire)
        {
            float interpZ = camera.nearClipPlane + FocusDistance * camera.farClipPlane;
            float focusZ = transform.position.z + interpZ;
            Vector3 fpos = new Vector3(transform.position.x, transform.position.y, focusZ);

            Gizmos.color = new Color(0, 1, 0, 0.2f);
            float fov = camera.fieldOfView / 2 * Mathf.Deg2Rad;
            float h = interpZ * Mathf.Tan(fov) * 2f;
            float w = h * camera.aspect;
            Gizmos.DrawCube(fpos, new Vector3(w, h, 0.01f));
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(fpos, new Vector3(w, h, 0.01f));
        }
    }
}