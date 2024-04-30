using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.Collections;
using UnityEditor.Sprites;
using FSM;

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