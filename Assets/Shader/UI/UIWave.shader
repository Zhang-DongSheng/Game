// Shader: UI水波纹
Shader "UI/Wave"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _Center ("Center", Vector) = (0, 0, 0, 0)
        _Amount ("Amount", Range(0, 1)) = 0.5
        _Wave ("Wave", Range(0, 200)) = 100
        _Speed ("Speed", Range(0, 500)) = 100
        _Precision ("Precision", Range(0.00001, 0.1)) = 0.1
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
            float4 _Center;
            fixed _Amount;
            float _Wave;
            float _Speed;
            float _Precision;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = _Center.xy;

                float2 uv = i.uv;

                float2 distance = center - uv;

                float length = sqrt(dot(distance, distance));

                float amount = _Amount / (0.01 + length * _Speed);

                if (amount < _Precision)
                {
                    amount = 0;
                }

                uv.y += amount * cos(length * _Wave * UNITY_PI);

                fixed4 col = tex2D(_MainTex, uv);

                return col;
            }
            ENDCG
        }
    }
}