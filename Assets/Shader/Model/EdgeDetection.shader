// ±ßÔµ¼ì²â
Shader "Model/EdgeDetection"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0, 0, 0, 1)
        _Range ("Range", Range(0.1, 3)) = 1
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

                fixed4 main = tex2D(_MainTex, i.uv[4]);

                fixed4 color = lerp(_Color, main, edge);

                color = lerp(main, color, _Edge);

                return color;
            }
            ENDCG
        }
    }
    FallBack Off
}