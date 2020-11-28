//遮罩shader
Shader "UI/Mask" 
{
    Properties 
    {
        [PerRendererData]_MainTex ("MainTex", 2D) = "white" {}
        _MaskTex ("MaskTex", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5

        //MASK SUPPORT ADD
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        //MASK SUPPORT END
    }
    SubShader 
    {
        Tags 
        {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        //MASK SUPPORT ADD
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]
        //MASK SUPPORT END
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Lighting Off
            ZWrite Off

            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma target 3.0

            #pragma vertex vert
            #pragma fragment frag

            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _MaskTex; uniform float4 _MaskTex_ST;

            struct a2v 
            {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };

            struct v2f 
            {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };

            v2f vert (a2v v)
            {
                v2f o = (v2f)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }

            float4 frag(v2f i) : SV_TARGET 
            {
                float4 _MainTex_var = tex2D(_MainTex, TRANSFORM_TEX(i.uv0, _MainTex));
                float3 finalColor = _MainTex_var.rgb;
                float4 _MaskTex_var = tex2D(_MaskTex, TRANSFORM_TEX(i.uv0, _MaskTex));
                return fixed4(finalColor, _MaskTex_var.a);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}