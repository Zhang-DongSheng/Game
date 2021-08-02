// ±ßÔµ¼ì²â[Roberts]
Shader "Model/EdgeDetection/Roberts"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _EdgeColor ("Edge Color", Color) = (0, 0, 0, 1)
        _Contrast ("Contrast Ratio", Range(0, 1)) = 1
        _Range ("Range", Float) = 1
        _Power ("Power", Float) = 1
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
            float _Power;
            float _Range;

            v2f vert (a2v v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                float2 uv = v.texcoord;

                o.uv[0] = uv + _MainTex_TexelSize.xy * float2(-1, -1) * _Range;
                o.uv[1] = uv + _MainTex_TexelSize.xy * float2( 1, -1) * _Range;
                o.uv[2] = uv + _MainTex_TexelSize.xy * float2(-1,  1) * _Range;
                o.uv[3] = uv + _MainTex_TexelSize.xy * float2( 1,  1) * _Range;
                o.uv[4] = uv;

                return o;
            }

            float Reberts(v2f i)
            {
                const float Gx[4] = 
                {
                    -1, 0,
                    0, 1,
                };
                const float Gy[4] = 
                {
                    0, -1,
                    1, 0,
                };

                float edgex, edgey, gray;

                for(int index = 0; index < 4; index++)
                {
                    gray = Luminance(tex2D(_MainTex, i.uv[index]).rgb);

                    edgex += gray * Gx[index];

                    edgey += gray * Gy[index];
                }
                return 1 - abs(edgex) - abs(edgey);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float edge = Reberts(i);
                 
                edge = pow(edge, _Power);

                fixed4 color = tex2D(_MainTex, i.uv[4]);

                color = lerp(_Color, color, _Contrast);

                color.rgb = lerp(_EdgeColor, color, edge);

                return color;
            }
            ENDCG
        }
    }
    FallBack Off
}