Shader "Particles/Dissolve"
{
    Properties
    {
        _DissolveColor("Dissolve Color", Color) = (0, 0, 0, 1)
		_DissolveEdgeColor("Dissolve Edge Color", Color) = (1, 1, 1, 1)
        _MainTex ("Base 2D", 2D) = "white" {}
        _DissolveMap ("DissolveMap", 2D) = "white" {}
        _ColorFactor("ColorFactor", Range(0, 1)) = 0.7
        _DissolveEdge("DissolveEdge", Range(0, 1)) = 0.8
        _DissolveThreshold("DissolveThreshold", Range(0, 1.01)) = 0
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

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            fixed4 _DissolveColor;
            fixed4 _DissolveEdgeColor;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _DissolveMap;
            float4 _DissolveMap_ST;
            float _DissolveThreshold;
            float _DissolveEdge;
            float _ColorFactor;

            v2f vert (appdata_base v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.normal = mul(v.normal, (float3x3)unity_WorldToObject);

                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 dissolve = tex2D(_DissolveMap, i.uv);

                clip(dissolve.r - _DissolveThreshold);

                fixed3 color = tex2D(_MainTex, i.uv).rgb;

                float percentage = _DissolveThreshold / dissolve.r;

                float lerpEdge = sign(percentage - _ColorFactor - _DissolveEdge);

                fixed3 edgeColor = lerp(_DissolveEdgeColor.rgb, _DissolveColor.rgb, saturate(lerpEdge));

                float lerpOut = sign(percentage - _ColorFactor);

                fixed3 final = lerp(color, edgeColor, saturate(lerpOut));

                return fixed4(final, 1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}