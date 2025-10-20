// ม๗นโ
Shader "UI/Shiny"
{
    Properties
    {
        [PerRendererData] _MainTex("Main Texture", 2D) = "white" {}
        _FlowTex("Flow Texture", 2D) = "white" {}
        _Color ("Color Tint", Color) = (1, 1, 1, 1)
        _Speed("Speed", Range(0, 100)) = 1
        _Space("Space", Range(2, 10)) = 2
        [MaterialToggle]_Direction("Direction", Range(0, 1)) = 1

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
 
            struct a2v
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };
 
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                half2 uvM : TEXCOORD0;
                half2 uvF: TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _FlowTex;
            float4 _FlowTex_ST;
            float4 _Color;
            fixed _Direction;
            half _Speed;
            half _Space;

            v2f vert(a2v i)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(i.vertex);

                o.color = i.color * _Color;

                o.uvM = TRANSFORM_TEX(i.texcoord, _MainTex);

                o.uvF = TRANSFORM_TEX(i.texcoord, _FlowTex);

                if (_Direction == 1 )
                    o.uvF.x += (frac(_Time.x * _Speed) - 0.5) * _Space;
                else
                    o.uvF.y += (frac(_Time.y * _Speed) - 0.5) * _Space;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 back = tex2D(_MainTex, i.uvM);

                float4 color = tex2D(_FlowTex, i.uvF) * i.color;

                color.a = back.a * color.a;

                clip(color.a - 0.01);

                return color;
            }
            ENDCG
        }
    }
    FallBack "VertexLit"
}