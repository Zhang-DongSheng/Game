Shader "Study/Example"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        [Enum(None, 0, First, 1, Second, 2)] _Enum ("Enum", Int) = 1
    }
    SubShader
    {
        Tags 
        {
            //"RenderType"="Opaque"               //���ڴ������ɫ��[������ɫ�����Է�����ɫ����������ɫ���Լ����ε���ɫ��]
            //"RenderType"="Transparent"          //���ڰ�͸����ɫ��[͸����ɫ����������ɫ����������ɫ�������ζ���ͨ������ɫ��]
            //"RenderType"="TransparentCutout"    //��Ƥ͸����ɫ��[Transparent Cutout������ͨ����ֲ����ɫ��]
            //"RenderType"="Background"           //��պ���ɫ����
            //"RenderType"="Overlay"              //������ɫ����������ɫ��
            //"RenderType"="TreeOpaque"           //���������е���Ƥ
            //"RenderType"="TreeTransparentCutout"//���������е���Ҷ
            //"RenderType"="TreeBillboard"        //���������еĹ������
            //"RenderType"="Grass"                //���������еĲ�
            //"RenderType"="GrassBillboard"       //����������еĹ���Ʋ�
        }
        LOD 100

        Pass
        {
            Tags
            {
                //"LightMode"="Normal"            //������ս����Ĺ�����ɫ��ͨ��
                //"LightMode"="Vertex"            //�ɵĶ���������ɫ��ͨ��
                //"LightMode"="VertexLM"          //�ɵĶ���������ɫ��ͨ�������Ҵ����ƶ��˹���ͼ
                //"LightMode"="ForwardBase"       //ǰ����Ⱦ����ͨ��
                //"LightMode"="ForwardAdd"        //ǰ����Ⱦ�����ع�ͨ��
                //"LightMode"="LightPrePassBase"  //���ӳٹ��ջ���ͨ��
                //"LightMode"="LightPrePassFinal" //���ӳٹ�������ͨ��
                //"LightMode"="ShadowCaster"      //��ӰͶ������������ɫ��ͨ��
                //"LightMode"="Deffered"          //�ӳ���ɫ��ͨ��
                //"LightMode"="Meta"              //���ڲ��������ʺͷ���ֵ����ɫ��ͨ����������ӳ�������
                //"LightMode"="MotionVectors"     //�˶�ʸ����Ⱦͨ��
                //"LightMode"="ScriptableRenderPipeline"              //�Զ���ű�ͨ��
                //"LightMode"="ScriptableRenderPipelineDefaultUnlit"  //����ģʽ������ΪĬ��δ����û�й�ģʽʱ�������Զ���ű��ܵ�
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Enum;

            v2f vert (appdata_base v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, i.uv);

                switch(_Enum)
                {
                    case 1:
                        color = color * 0.5;
                        break;
                    case 2:
                        color = color * 0.75;
                        break;
                    default:
                        break;
                }

                return color;
            }
            ENDCG
        }
    }
}
