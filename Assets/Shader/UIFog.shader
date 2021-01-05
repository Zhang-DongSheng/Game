Shader "UI/Fog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Fog Color", Color) = (1, 1, 1, 1)
        _Speed ("Speed", Range(0, 10)) = 1
        _Scale ("Scale", Range(0, 10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Speed;
            float _Scale;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            float rand(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }

            float noise(float2 uv)
            {
                float2 i = floor(uv);

                float2 f = frac(uv);

                float a = rand(i);

                float b = rand(i + float2(1.0, 0.0));

                float c = rand(i + float2(0.0, 1.0));

                float d = rand(i + float2(1.0, 1.0));

                float2 u = f * f * f * (f * (f * 6 - 15) + 10);
                
                float x1 = lerp(a, b, u.x);

                float x2 = lerp(c, d, u.x);

                return lerp(x1, x2, u.y);
            }

            float compute(float2 uv)
            {
                float scale = _Scale;

                float res = 0;

                float w = 4;

                for(int i = 0; i < 4; ++i)
                {
                    res += noise(uv * w);

                    w *= 1.5;
                }
                return res * scale;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 col = tex2D(_MainTex, i.uv);

                float noise = compute(i.uv * abs(0.5 - frac(_Time.x)) * _Speed);

                float3 col_fog = noise * _Color;

                float3 col_out = lerp(col, col_fog, 0.2);

                return fixed4(col_out, 1);
            }
            ENDCG
        }
    }
}
