using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace UnityEditor.Listener
{
    public static class FBXListener
    {
        const string ANIMATORPATH = "/Animator";

        public static void Start(ModelImporter model)
        {
            OnImportFBX(model.assetPath);
        }

        private static void OnImportFBX(string path)
        {

        }
    }
}