Shader "UI/Unlit/FlowTexture"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _FlowTex("Fog Texture", 2D) = "white" {}
        _Color("Color Tint", Color) = (1, 1, 1, 1)
        _FlowlightColor("Flowlight Color", Color) = (1, 1, 1, 1)
        _MoveSpeed("Speed", Float) = 1
        [MaterialToggle] PixelSnap("Pixel snap", float) = 0
        /* UI */
        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
        _ColorMask("Color Mask", Float) = 15
        /* -- */
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

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha
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
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                half2 texcoord : TEXCOORD0;
                float4 worldPosition: TEXCOORD1;
            };

            fixed4 _Color;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.worldPosition = IN.vertex;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap(OUT.vertex);
                #endif

                return OUT;
            }

            sampler2D _MainTex;
            float4 _MainTex_ST;
            /* FlowTex */
            sampler2D _FlowTex;
            float _MoveSpeed;
            float _AmScale;
            fixed4 _FlowlightColor;
            float _Width;
            /* --------- */
            //如果使用图集中的图片，需要设置一下参数
            float _WidthRate;
            float _XOffset;
            float _HeightRate;
            float _YOffset;
            float2 _Tiling;

            bool _UseClipRect;
            float4 _ClipRect;
            float _ClipSoftX;
            float _ClipSoftY;
            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, IN.texcoord);

                /*使用裁剪*/
                if (_UseClipRect)
                {
                    float2 factor = float2(0.0, 0.0);
                    float2 tempXY = (IN.worldPosition.xy - _ClipRect.xy) / float2(_ClipSoftX, _ClipSoftY)*step(_ClipRect.xy, IN.worldPosition.xy);
                    factor = max(factor, tempXY);
                    float2 tempZW = (_ClipRect.zw - IN.worldPosition.xy) / float2(_ClipSoftX, _ClipSoftY)*step(IN.worldPosition.xy, _ClipRect.zw);
                    factor = min(factor, tempZW);
                    c.a *= clamp(min(factor.x, factor.y), 0.0, 1.0);
                }
                /* --------- */

                /* FlowTex*/
                //将UV由图集中的变换到0-1
                float2 uv = (IN.texcoord - float2(_XOffset, _YOffset)) / float2(_WidthRate, _HeightRate);
                //添加正弦扰动
                float offsetY = sin(uv.x * 6.28)*_AmScale;

                float offsetX = 0.5 - frac(_Time.x);

                uv += float2(offsetX * _MoveSpeed, offsetY) * _Tiling;
                //轮廓的限制，以0.5为中心，上下扩展
                fixed temp = step(abs(uv.y - 0.5*_Tiling.y), _Width*0.5*_Tiling.y);
                //对烟雾图进行采样
                fixed4 cadd = tex2D(_FlowTex,uv)*temp;
                /*
                //使用blend src，1-src
                c.rgb = cadd.rgb*cadd.a* _FlowlightColor +c.rgb*(1-cadd.a);
                */
                //使用blend add
                c.rgb = cadd.rgb * cadd.a * _FlowlightColor + c.rgb;
                /* --------- */
                c.rgb *= c.a;
                return c;
            }
            ENDCG
        }
    }
}