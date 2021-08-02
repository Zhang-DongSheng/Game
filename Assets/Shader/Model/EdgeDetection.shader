// ±ßÔµ¼ì²â
Shader "Unlit/EdgeDetection"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0, 0, 0, 1)
        _Edge ("Edge Only", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            ZTest Always
            Cull Off
            ZWrite Off
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
            float _Edge;

            v2f vert (a2v v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                float2 uv = v.texcoord;

                const fixed side[3] = 
                {
                    -1, 0, 1
                };

                int index = 0, count = 3;

                for(int i = 0; i < count; i++)
                {
                    for(int j = 0; j < count; j++)
                    {
                        index = i * count + j;

                        o.uv[index] = uv + _MainTex_TexelSize.xy * float2(side[j], side[i]);
                    }
                }
                return o;
            }

            fixed Gray(fixed4 color)
            {
                return 0.299 * color.r + 0.587 * color.g + 0.114 * color.b;
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

                float edgex, edgey, color;

                for(int index = 0; index < 9; index++)
                {
                    color = Gray(tex2D(_MainTex, i.uv[index]));

                    edgex += color * Gx[index];

                    edgey += color * Gy[index];
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