// ±ßÔµ¼ì²â[Sobel]
Shader "Model/EdgeDetection/Sobel"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _EdgeColor("Edge Color", Color) = (0, 0, 0, 1)
        _Contrast ("Contrast Ratio", Range(0, 1)) = 1
        _Range ("Range", Float) = 1
        _Edge ("Edge Only", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            ZTest Always
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct a2v
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv[9] : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            fixed4 _Color;
            fixed4 _EdgeColor;
            float _Contrast;
            float _Range;
            float _Edge;

            v2f vert (a2v v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                float2 uv = v.texcoord;

                o.uv[0] = uv + _MainTex_TexelSize.xy * float2(-1, -1) * _Range;
                o.uv[1] = uv + _MainTex_TexelSize.xy * float2( 0, -1) * _Range;
                o.uv[2] = uv + _MainTex_TexelSize.xy * float2( 1, -1) * _Range;
                o.uv[3] = uv + _MainTex_TexelSize.xy * float2(-1,  0) * _Range;
                o.uv[4] = uv + _MainTex_TexelSize.xy * float2( 0,  0) * _Range;
                o.uv[5] = uv + _MainTex_TexelSize.xy * float2( 1,  0) * _Range;
                o.uv[6] = uv + _MainTex_TexelSize.xy * float2(-1,  1) * _Range;
                o.uv[7] = uv + _MainTex_TexelSize.xy * float2( 0,  1) * _Range;
                o.uv[8] = uv + _MainTex_TexelSize.xy * float2( 1,  1) * _Range;

                return o;
            }

            float Sobel(v2f i)
            {
                const float Gx[9] = 
                {
                    -1, -2, -1,
                    0, 0, 0,
                    1, 2, 1,
                };
                const float Gy[9] = 
                {
                    -1, 0, 1,
                    -2, 0, 2,
                    -1, 0, 1,
                };

                float edgex, edgey, gray;

                for(int index = 0; index < 9; index++)
                {
                    gray = Luminance(tex2D(_MainTex, i.uv[index]).rgb);

                    edgex += gray * Gx[index];

                    edgey += gray * Gy[index];
                }
                return 1 - abs(edgex) - abs(edgey);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float edge = Sobel(i);

                fixed4 color = tex2D(_MainTex, i.uv[4]);

                color = lerp(_Color, color, _Contrast);

                fixed4 final = lerp(_EdgeColor, color, edge);

                final = lerp(color, final, _Edge);

                return final;
            }
            ENDCG
        }
    }
    FallBack Off
}