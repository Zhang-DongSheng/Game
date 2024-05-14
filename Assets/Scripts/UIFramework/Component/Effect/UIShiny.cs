namespace UnityEngine.UI
{
    [RequireComponent(typeof(Graphic))]
    public class UIShiny : MonoBehaviour
    {
        private Graphic graphic;

        private Material material;

        private void Awake()
        {
            graphic = GetComponent<Graphic>();

            //material = new Material(Shader.Find("UI/Shiny"));

            //graphic.material = material;
        }

        private void Update()
        {
            //material
        }
    }
}