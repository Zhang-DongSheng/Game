/* * �﷨˵��
#pragma multi_compile_instancing    ʹ�ø�ָ����Unity����ʵ���ı��壬�ڱ�����ɫ�����ǲ���Ҫ��
UNITY_VERTEX_INPUT_INSTANCE_ID      �ڶ�����ɫ������������ṹ�ж���ʵ��Id
UNITY_INSTANCING_BUFFER_START(name) / UNITY_INSTANCING_BUFFER_END(name)	 ÿ��ʵ�����Զ�Ҫ��һ�������������ж��壬ʹ����һ�Ժ������ϣ����ÿ��ʵ����ֵ����һ�������԰�����
UNITY_DEFINE_INSTANCED_PROP(propertyType, propertyName)	    ʹ�����ͺ����ƶ���һ��ʵ������
UNITY_SETUP_INSTANCE_ID(v);         ʹshader�������Է���ʵ��Id ���ڶ�����ɫ���б���ʹ�ã�ƬԪ��ɫ����ѡ
UNITY_TRANSFER_INSTANCE_ID(v, o);   �ڶ�����ɫ���н�ʵ��Id������ṹ���Ƶ�����ṹ��ֻ����ƬԪ��ɫ����Ҫ����ʵ������ʱ����Ҫ
UNITY_ACCESS_INSTANCED_PROP(arrayName, propertyName)	    �ӻ������л�ȡʵ��������ֵ��arrayName��UNITY_INSTANCING_BUFFER_START(name)��Ӧ
* */
Shader "Study/GPUInstance"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct a2v
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(a2v v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, i.uv);

                return color;
            }
            ENDCG
        }
    }
}