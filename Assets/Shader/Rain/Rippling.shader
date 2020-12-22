Shader "Rain/Rippling"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed ("Speed", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct a2v
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Speed;

            v2f vert (a2v v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            float mask(fixed step, fixed2 offset)
            {
                fixed3 emissive = 1 - frac((_Time * _Speed) + step);

                fixed3 base = tex2D(_MainTex, offset).rgb;

                return saturate(1 - distance(emissive.r - base.r, 0.05) / 0.05);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed mask1 = mask(0, i.uv);

                fixed mask2 = mask(0.5, i.uv + fixed2(0.5, 0.5));

                fixed progress = saturate(abs(sin(_Time * 0.5)));

                fixed final = lerp(mask1, mask2, progress);

                return fixed4(final, final, final, 1);
            }
            ENDCG
        }
    }
}
