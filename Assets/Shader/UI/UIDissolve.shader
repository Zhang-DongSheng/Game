// �ܽ�
Shader "UI/Dissolve"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}                        // ������
        _NoiseTex("Noise Texture", 2D) = "white" {}                 // ��������
        _DissolveThreshold("Dissolve Threshold", Range(0, 1)) = 0.5 // �ܽ���ֵ
        _EdgeColor("Edge Color", Color) = (1, 0, 0, 1)              // ��Ե��ɫ
        _EdgeWidth("Edge Width", Range(0, 0.2)) = 0.1               // ��Ե���

        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
        _ColorMask("Color Mask", Float) = 15
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "CanUseSpriteAtlas" = "True"
        }
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask[_ColorMask]

        Stencil
        {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask]
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
    
            #include "UnityCG.cginc"
    
            // ��������ṹ
            struct appdata
            {
                float4 vertex : POSITION;  // ����λ��
                float2 uv : TEXCOORD0;     // ��������
            };
    
            // ��������ṹ
            struct v2f
            {
                float2 uv : TEXCOORD0;      // ����������
                float2 uvNoise : TEXCOORD1; // ������������
                float4 vertex : SV_POSITION;// �ü��ռ䶥��λ��
            };
    
            // ���Ա���
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            float _DissolveThreshold;
            float4 _EdgeColor;
            float _EdgeWidth;
    
            // ���� Shader
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);  // ����任���ü��ռ�
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);       // ����������任
                o.uvNoise = TRANSFORM_TEX(v.uv, _NoiseTex); // ������������任
                return o;
            }
    
            // Ƭ�� Shader
            fixed4 frag(v2f i) : SV_Target
            {
                // ����������
                fixed4 main = tex2D(_MainTex, i.uv);
                // ������������
                float noise = tex2D(_NoiseTex, i.uvNoise).r;
                // �ܽ�Ч�� �������ֵС����ֵ����������
                if (noise < _DissolveThreshold)
                    discard;
                // �����Ե͸����
                float edgeAlpha = smoothstep(_DissolveThreshold, _DissolveThreshold + _EdgeWidth, noise);
                // �����Ե��ɫ
                fixed4 edgeColor = _EdgeColor * (1 - edgeAlpha);
                // ������ɫ = ��������ɫ + ��Ե��ɫ
                fixed4 col = main * edgeAlpha + edgeColor;
                // ����͸����
                col.a = min(main.a, edgeAlpha);

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}