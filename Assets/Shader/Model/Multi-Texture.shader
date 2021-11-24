Shader "Model/Multi-Texture"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _MainTex ("First (RGB)", 2D) = "white" {}
        _SecondTex ("Second (RGB)", 2D) = "white" {}
        _ThirdTex ("Third (RGB)", 2D) = "white" {}
        _Fixed ("Fixed", Range(0, 1)) = 0.0
    }
    SubShader
    {
        Tags 
        {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #include "UnityCG.cginc"
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma target3.0

            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
            };
            struct v2f
            {
                float4 vertex : SV_Position;
                float2 uv[3] : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _SecondTex;
            float4 _SecondTex_ST;
            sampler2D _ThirdTex;
            float4 _ThirdTex_ST;
            fixed4 _Color;
            float _Fixed;

            v2f vert(a2v v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv[0] = TRANSFORM_TEX(v.texcoord, _MainTex);

                o.uv[1] = TRANSFORM_TEX(v.texcoord, _SecondTex);

                o.uv[2] = TRANSFORM_TEX(v.texcoord, _ThirdTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 main = tex2D(_MainTex, i.uv[0]) * _Color;

                fixed4 extra = tex2D(_SecondTex, i.uv[1]) * tex2D(_ThirdTex, i.uv[2]);

                fixed4 final = lerp(main, extra, _Fixed);

                return final;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}