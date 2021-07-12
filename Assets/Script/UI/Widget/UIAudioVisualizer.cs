using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIAudioVisualizer : MonoBehaviour
    {
        [SerializeField] private AudioVisualizer visualizer;

        [SerializeField, Range(0, 10)] private float H = 5;

        [SerializeField, Range(0, 1f)] private float S = 1;

        [SerializeField, Range(0, 1f)] private float V = 1;

        [SerializeField] private float height = 1000f;

        [SerializeField] private List<Image> images;

        private RectTransform target;

        private void Awake()
        {
            visualizer.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(int index, float value)
        {
            if (images.Count > index)
            {
                if (images[index].TryGetComponent(out target))
                {
                    target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value * height);
                }
                images[index].color = Color.HSVToRGB(value * H, S, V);
            }
        }
    }
}