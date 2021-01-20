﻿using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using System.IO;
namespace BDSZ_2020
{
    class BDSZ_UIEffectCreateMenu : ScriptableObject
    {


      
       
        // ReSharper disable once InconsistentNaming
        private static void PlaceUIElementRoot(GameObject element, MenuCommand menuCommand)
        {
            GameObject parent = menuCommand.context as GameObject;
            if (parent == null || parent.GetComponentInParent<Canvas>() == null)
            {
                parent = GetOrCreateCanvasGameObject();
            }
            string uniqueName = GameObjectUtility.GetUniqueNameForSibling(parent.transform, element.name);
            element.name = uniqueName;
            Undo.RegisterCreatedObjectUndo(element, "Create " + element.name);
            Undo.SetTransformParent(element.transform, parent.transform, "Parent " + element.name);
            GameObjectUtility.SetParentAndAlign(element, parent);
            if (parent != menuCommand.context) // not a context click, so center in sceneview
                SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), element.GetComponent<RectTransform>());

            Selection.activeGameObject = element;
        }


        private static GameObject GetOrCreateCanvasGameObject()
        {
            GameObject selectedGo = Selection.activeGameObject;

            // Try to find a gameobject that is the selected GO or one if its parents.
            Canvas canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
            if (canvas != null && canvas.gameObject.activeInHierarchy)
                return canvas.gameObject;

            // No canvas in selection or its parents? Then use just any canvas..
            canvas = Object.FindObjectOfType(typeof(Canvas)) as Canvas;
            if (canvas != null && canvas.gameObject.activeInHierarchy)
                return canvas.gameObject;

            // No canvas in the scene at all? Then create a new one.
            return CreateNewUI();
        }

        private static void SetPositionVisibleinSceneView(RectTransform canvasRTransform, RectTransform itemTransform)
        {
            // Find the best scene view
            SceneView sceneView = SceneView.lastActiveSceneView;
            if (sceneView == null && SceneView.sceneViews.Count > 0)
                sceneView = SceneView.sceneViews[0] as SceneView;

            // Couldn't find a SceneView. Don't set position.
            if (sceneView == null || sceneView.camera == null)
                return;

            // Create world space Plane from canvas position.
            Vector2 localPlanePosition;
            Camera camera = sceneView.camera;
            Vector3 position = Vector3.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2(camera.pixelWidth / 2f, camera.pixelHeight / 2f), camera, out localPlanePosition))
            {
                // Adjust for canvas pivot
                localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
                localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;

                localPlanePosition.x = Mathf.Clamp(localPlanePosition.x, 0, canvasRTransform.sizeDelta.x);
                localPlanePosition.y = Mathf.Clamp(localPlanePosition.y, 0, canvasRTransform.sizeDelta.y);

                // Adjust for anchoring
                position.x = localPlanePosition.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
                position.y = localPlanePosition.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;

                Vector3 minLocalPosition;
                minLocalPosition.x = canvasRTransform.sizeDelta.x * (0 - canvasRTransform.pivot.x) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
                minLocalPosition.y = canvasRTransform.sizeDelta.y * (0 - canvasRTransform.pivot.y) + itemTransform.sizeDelta.y * itemTransform.pivot.y;

                Vector3 maxLocalPosition;
                maxLocalPosition.x = canvasRTransform.sizeDelta.x * (1 - canvasRTransform.pivot.x) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
                maxLocalPosition.y = canvasRTransform.sizeDelta.y * (1 - canvasRTransform.pivot.y) - itemTransform.sizeDelta.y * itemTransform.pivot.y;

                position.x = Mathf.Clamp(position.x, minLocalPosition.x, maxLocalPosition.x);
                position.y = Mathf.Clamp(position.y, minLocalPosition.y, maxLocalPosition.y);
            }

            itemTransform.anchoredPosition = position;
            itemTransform.localRotation = Quaternion.identity;
            itemTransform.localScale = Vector3.one;
        }

        // ReSharper disable once InconsistentNaming
        private static GameObject CreateNewUI()
        {
            // Root for the UI
            var root = new GameObject("Canvas")
            {
                layer = LayerMask.NameToLayer("UI")
            };
            Canvas canvas = root.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            root.AddComponent<CanvasScaler>();
            root.AddComponent<GraphicRaycaster>();
            Undo.RegisterCreatedObjectUndo(root, "Create " + root.name);

            // if there is no event system add one...
            CreateEventSystem(false);
            return root;
        }

        private static void CreateEventSystem(bool select, GameObject parent = null)
        {
            var esys = Object.FindObjectOfType<EventSystem>();
            if (esys == null)
            {
                var eventSystem = new GameObject("EventSystem");
                GameObjectUtility.SetParentAndAlign(eventSystem, parent);
                esys = eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
                Undo.RegisterCreatedObjectUndo(eventSystem, "Create " + eventSystem.name);
            }

            if (select && esys != null)
            {
                Selection.activeGameObject = esys.gameObject;
            }
        }

         
        static public GameObject FindParentParticleObject(MenuCommand menuCommand,System.Type t)
        {
            GameObject selectedGo = Selection.activeGameObject;
            if (selectedGo != null)
            {

                // Try to find a gameobject that is the selected GO or one if its parents.
                Component  ec = selectedGo.GetComponentInParent(t);
                if (ec != null && ec.gameObject.activeInHierarchy)
                    return ec.gameObject;
            }
            Component ec1 = GameObject.FindObjectOfType(t) as Component;
            if (ec1 != null)
                return ec1.gameObject;
            CreateUIEmitterParticle(menuCommand);
            return Selection.activeGameObject;

        }
        static public GameObject FindParentGameObject(MenuCommand menuCommand)
        {

            GameObject selectedGo = Selection.activeGameObject;
            if (selectedGo != null)
            {

                // Try to find a gameobject that is the selected GO or one if its parents.
                BDSZ_UIEffectController ec = selectedGo.GetComponentInParent<BDSZ_UIEffectController>();
                if (ec != null && ec.gameObject.activeInHierarchy)
                    return ec.gameObject;

                BDSZ_ParticleRoot customRender1 = selectedGo.GetComponent(typeof(BDSZ_ParticleRoot)) as BDSZ_ParticleRoot;
                if (customRender1 != null)
                {
                    GameObject go1 = new GameObject("Controller");
                    GameObjectUtility.SetParentAndAlign(go1, customRender1.gameObject);
                    BDSZ_UIEffectController effect1 = go1.AddComponent<BDSZ_UIEffectController>();
                    //  customRender.AddEffect(effect);
                    return go1;
                }
            }

            BDSZ_UIParticlesResMgr.Prepare();

            GameObject customRender = new GameObject("ParticleRoot");
            customRender.AddComponent<RectTransform>().sizeDelta = new Vector2(Screen.width/2, Screen.height/2);
            BDSZ_ParticleRoot line = customRender.AddComponent<BDSZ_ParticleRoot>();
            line.raycastTarget = false;
            PlaceUIElementRoot(customRender, menuCommand);
            GameObject go = new GameObject("Controller");
            GameObjectUtility.SetParentAndAlign(go, customRender);
            BDSZ_UIEffectController effect = go.AddComponent<BDSZ_UIEffectController>();
            //  customRender.AddEffect(effect);
            return go;


        }
        static public GameObject CreateEffectObject(System.Type t, GameObject parent, string effectName)
        {
            GameObject child = new GameObject(effectName);
            // child.layer = LayerMask.NameToLayer(UIInternalUtility.c_strUILayer);
            Undo.RegisterCreatedObjectUndo(child, "Create " + effectName);
            GameObjectUtility.SetParentAndAlign(child, parent);
            if (parent != null)
            {
                //child.layer = UIInternalUtility.UILayerMask;
                Undo.SetTransformParent(child.transform, parent.transform, "Parent " + child.name);
                child.transform.parent = parent.transform;//
                child.layer = parent.layer;
            }
            BDSZ_EffectBase effectBase = child.AddComponent(t) as BDSZ_EffectBase;
            if (effectBase != null)
            {
                Material material = AssetDatabase.GetBuiltinExtraResource<Material>("Default-ParticleSystem.mat");
                if (material != null)
                {
                    effectBase.SetMainTexture(material.mainTexture);
                }
                effectBase.OnEditorCreate();
            }
            return child;
        }
        //[MenuItem("UI/Effect/Particle")]
        [MenuItem("GameObject/UIEffect/Particle", false, 3101)]
        static public void CreateUIParticle(MenuCommand menuCommand)
        {

            GameObject parentGO = FindParentGameObject(menuCommand);
            if (parentGO != null)
            {
                GameObject child = CreateEffectObject(typeof(BDSZ_UIParticle), parentGO, "Particle");
                Selection.activeGameObject = child;
            }
        }


        [MenuItem("GameObject/UIEffect/EmitterParticle",false,3102)]
        static public void CreateUIEmitterParticle(MenuCommand menuCommand)
        {

            GameObject parentGO = FindParentGameObject(menuCommand);
            if (parentGO != null)
            {
                GameObject child = CreateEffectObject(typeof(BDSZ_UIEmitterParticles), parentGO, "EmitterParticle");
                Selection.activeGameObject = child;
            }
        }
        [MenuItem("GameObject/UIEffect/CircleCurve", false, 3202)]
        static public void CreateUICircleCurve(MenuCommand menuCommand)
        {
            GameObject parentGO = FindParentParticleObject(menuCommand, typeof(BDSZ_UIEmitterParticles));
            if (parentGO != null)
            {
                GameObject child = CreateEffectObject(typeof(BDSZ_UICircleCurve), parentGO, "CircleCurve");
                Selection.activeGameObject = child;
            }
        }
        [MenuItem("GameObject/UIEffect/RectCurve", false, 3203)]
        static public void CreateUIRectCurve(MenuCommand menuCommand)
        {
            GameObject parentGO = FindParentParticleObject(menuCommand, typeof(BDSZ_UIEmitterParticles));
            if (parentGO != null)
            {
                GameObject child = CreateEffectObject(typeof(BDSZ_UIRectCurve), parentGO, "RectCurve");
                Selection.activeGameObject = child;
            }
        }

        [MenuItem("GameObject/UIEffect/WindForce", false, 3301)]
        static public void CreateMoveWind(MenuCommand menuCommand)
        {
            GameObject parentGO = FindParentParticleObject(menuCommand, typeof(BDSZ_UIEffectController));
            if (parentGO != null)
            {
                GameObject uc = parentGO.GetComponentInParent<BDSZ_UIEffectController>().gameObject;
                GameObject child = CreateEffectObject(typeof(BDSZ_EffectWindForce), uc, "Wind");
                Selection.activeGameObject = child;
            }
        }
    }
}