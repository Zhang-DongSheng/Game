using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    public Texture2D[] source;

    public Image[] img_test;

    DynamicAtlasPacker packer;

    private void Awake()
    {
        packer = new DynamicAtlasPacker();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            int index = UnityEngine.Random.Range(0, 5);

            Debug.LogError(index);

            img_test[index].overrideSprite = packer.Pop("TE", source[index].name);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            packer.Push("TE", source[0].name, source[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            packer.Push("TE", source[1].name, source[1]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            packer.Push("TE", source[2].name, source[2]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            packer.Push("TE", source[3].name, source[3]);
        }
    }
}