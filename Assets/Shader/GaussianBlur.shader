Shader "UI/Gaussian Blur"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        _Blur("Blur",Range(0,1)) = 0.01                                 // 模糊程度
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0

        //MASK SUPPORT ADD
        [HideInInspector]_StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector]_Stencil ("Stencil ID", Float) = 0
        [HideInInspector]_StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector]_StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector]_StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector]_ColorMask ("Color Mask", Float) = 15
        //MASK SUPPORT END
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
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
        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha
    
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
    
            struct a2v
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };
    
            struct v2f
            {
                float4 pos : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                fixed alpha : TEXCOORD1;
            };
            
            sampler2D _MainTex;
            sampler2D _AlphaTex;
            float _AlphaSplitEnabled;
            fixed4 _Color;
            float _Blur;
    
            v2f vert(a2v i)
            {
                v2f o;

                o.pos = UnityObjectToClipPos(i.vertex);

                o.color = i.color;

                o.texcoord = i.texcoord;
    
                return o;
            }
    
            fixed4 SampleSpriteTexture(float2 uv)
            {
                // 1 / 16
                float offset = _Blur * 0.0625f;
                // 左上
                fixed4 color = tex2D(_MainTex, float2(uv.x - offset, uv.y - offset)) * 0.0947416f;
                // 上
                color += tex2D(_MainTex, float2(uv.x, uv.y - offset)) * 0.118318f;
                // 右上
                color += tex2D(_MainTex, float2(uv.x + offset, uv.y + offset)) * 0.0947416f;
                // 左
                color += tex2D(_MainTex, float2(uv.x - offset, uv.y)) * 0.118318f;
                // 中
                color += tex2D(_MainTex, float2(uv.x, uv.y)) * 0.147761f;
                // 右
                color += tex2D(_MainTex, float2(uv.x + offset, uv.y)) * 0.118318f;
                // 左下
                color += tex2D(_MainTex, float2(uv.x - offset, uv.y + offset)) * 0.0947416f;
                // 下
                color += tex2D(_MainTex, float2(uv.x, uv.y + offset)) * 0.118318f;
                // 右下
                color += tex2D(_MainTex, float2(uv.x + offset, uv.y - offset)) * 0.0947416f;
    
    
#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
                if (_AlphaSplitEnabled)
                    color.a = tex2D(_AlphaTex, uv).r;
#endif

                return color;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color = SampleSpriteTexture(i.texcoord) * i.color;

                color.rgb *= color.a;

                return color;
            }
            ENDCG
        }
    }
}