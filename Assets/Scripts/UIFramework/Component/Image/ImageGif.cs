using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace UnityEngine.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class ImageGif : MonoBehaviour
    {
        [SerializeField] private string path = "Gif/wukelan.gif";

        [SerializeField] private float speed = 5f;

        private Image component;

        private readonly List<Sprite> sprites = new List<Sprite>();

        private int index, count;

        private float timer;

        private void Awake()
        {
            component = GetComponent<Image>();

            string url = string.Format("{0}/{1}", Application.streamingAssetsPath, path);

            StartCoroutine(Reload(url));
        }

        private IEnumerator Reload(string path)
        {
            var request = UnityWebRequest.Get(path);

            yield return request.SendWebRequest();

            var buffer = request.downloadHandler.data;

            var image = Game.Utility.Image.Deserialize(buffer);

            sprites.Clear();

            sprites.AddRange(Game.Utility.Image.ImageToSprites(image));

            count = sprites.Count;
        }

        private void Update()
        {
            if (component == null) return;

            if (timer > count)
                timer = 0;
            else
                timer += Time.deltaTime * speed;

            if (count > 1)
            {
                index = (int)timer % count;

                component.sprite = sprites[index];
            }
        }

        private void OnDestroy()
        {
            sprites.Clear();
        }
    }
}