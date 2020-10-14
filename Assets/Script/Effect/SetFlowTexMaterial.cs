using UnityEngine.UI;
using UnityEngine;
/// <summary>
/// 可以在UGUI的image或者raw image使用
/// </summary>
public class SetFlowTexMaterial : MonoBehaviour
{
    private float widthRate = 1;
    private float heightRate = 1;
    private float xOffsetRate = 0;
    private float yOffsetRate = 0;
    private MaskableGraphic maskableGraphic;
    public Vector2 tiling = Vector2.one;
    public float amScale = 0.05f;
    public float width = 1;
    void Awake()
    {
        maskableGraphic = GetComponent<MaskableGraphic>();
        if (maskableGraphic)
        {
            Image image = maskableGraphic as Image;
            if (image)
            {
                image.material = new Material(Shader.Find("UI/Unlit/AddFlowTex"));
                widthRate = image.sprite.textureRect.width * 1.0f / image.sprite.texture.width;
                heightRate = image.sprite.textureRect.height * 1.0f / image.sprite.texture.height;
                xOffsetRate = (image.sprite.textureRect.xMin) * 1.0f / image.sprite.texture.width;
                yOffsetRate = (image.sprite.textureRect.yMin) * 1.0f / image.sprite.texture.height;
            }
        }
        Debug.Log(string.Format(" widthRate{0}, heightRate{1}， xOffsetRate{2}， yOffsetRate{3}", widthRate, heightRate, xOffsetRate, yOffsetRate));
    }
    void Start()
    {
        SetShader();
    }
    ////调试使用，可看实时效果，项目中建议关闭
    //void Update()
    //{
    //    SetShader();
    //}
    public void SetShader()
    {
        maskableGraphic.material.SetVector("_Tiling", tiling);
        maskableGraphic.material.SetFloat("_AmScale", amScale);
        maskableGraphic.material.SetFloat("_WidthRate", widthRate);
        maskableGraphic.material.SetFloat("_HeightRate", heightRate);
        maskableGraphic.material.SetFloat("_XOffset", xOffsetRate);
        maskableGraphic.material.SetFloat("_YOffset", yOffsetRate);
        maskableGraphic.material.SetFloat("_ClipSoftX", 10);
        maskableGraphic.material.SetFloat("_ClipSoftY", 10);
        maskableGraphic.material.SetFloat("_Width", width);
    }

}
