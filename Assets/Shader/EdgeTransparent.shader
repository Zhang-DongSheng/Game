Shader "Test/EdgeTransparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Circle ("Circle", Range(0, 1)) = 0.1
        _Ratio ("Ratio", Range(0, 5)) = 0.1
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "LightMode"="ForwardBase"
        }
        ZWrite Off ZTest Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Circle;
            float _Ratio;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float2 side = float2(0.5f, 0.5f) - i.uv;

                float distance = pow(side.x, 2) + pow(side.y, 2);

                if(distance > _Circle)
                    col.a = max(0, 1 - (distance - _Circle) * _Ratio);
                return col;
            }
            ENDCG
        }
    }
}
