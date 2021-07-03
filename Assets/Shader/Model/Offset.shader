Shader "Model/Offset"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OffsetMap ("UV Offset Map(A)", 2D) = "white" {}
        _Amount ("UV Offset Amount", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _OffsetMap;
            float4 _OffsetMap_ST;
            float _Amount;

            v2f vert (appdata v)
            {
                v2f o;

                o.pos = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float v1 = tex2D(_OffsetMap, i.uv).a;

                float v2 = (v1 * 2 - 1) * _Amount;

                float2 offset = float2(v2, v2);

                i.uv += offset;

                fixed4 color = tex2D(_MainTex, i.uv);

                return color;
            }
            ENDCG
        }
    }
}