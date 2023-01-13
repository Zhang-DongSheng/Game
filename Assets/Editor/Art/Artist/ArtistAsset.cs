using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Window
{
    public partial class Artist : CustomWindow
    {
        private readonly string[] assets = new string[6] { "None", "TextAsset", "Texture", "Sprite", "Shader", "Material" };

        private readonly string[] shaders = new string[2] { "Reference", "Find" };

        private readonly Index indexAsset = new Index();

        private readonly Index indexShader = new Index();

        private readonly Input inputShader = new Input();

        private void RefreshAsset()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    indexAsset.value = EditorGUILayout.Popup(indexAsset.value, assets);
                }
                GUILayout.EndVertical();

                GUILayout.Box(string.Empty, GUILayout.Width(3), GUILayout.ExpandHeight(true));

                GUILayout.BeginVertical(GUILayout.Width(200));
                {
                    switch (indexAsset.value)
                    {
                        case 0:
                            {
                                GUILayout.Label(string.Empty);
                            }
                            break;
                        case 2:
                        case 3:
                            {
                                if (GUILayout.Button("���ͼƬ��Դ�Ƿ�Ϊ4�ı���"))
                                {
                                    AssetDetection.Powerof2("Assets");
                                }
                                goto default;
                            }
                        case 4:
                            {
                                indexShader.value = EditorGUILayout.Popup(indexShader.value, shaders);

                                switch (indexShader.value)
                                {
                                    case 0:
                                        {
                                            if (GUILayout.Button("��������δ�����õ�Shader"))
                                            {
                                                AssetDetection.FindUnreferencedShader();
                                            }
                                            if (GUILayout.Button("�������б����õ�Shader"))
                                            {
                                                AssetDetection.FindReferenceShader();
                                            }
                                        }
                                        break;
                                    case 1:
                                        {
                                            inputShader.value = GUILayout.TextField(inputShader.value);

                                            if (GUILayout.Button("�������ø���Դ������Material"))
                                            {
                                                AssetDetection.FindMaterialOfReferenceShader(inputShader.value);
                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                        default:
                            {
                                if (GUILayout.Button("�����Դ����"))
                                {
                                    FindReferences.Empty(string.Format("t:{0}", assets[indexAsset.value]), "Assets");
                                }
                                if (GUILayout.Button("�����Դ��С"))
                                {
                                    AssetDetection.Overflow(string.Format("t:{0}", assets[indexAsset.value]), "Assets");
                                }
                            }
                            break;
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
    }
}