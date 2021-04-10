// Shader: ¾¶ÏòÄ£ºý
Shader "Blur/Radial"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}

        _FocusPos ("Focus Position", Vector) = (0, 0, 0, 0)

        _SampleNumer ("Sample Number", Range(0, 10)) = 1

        _ScaleFactor ("Scale Factor", Range(0, 1)) = 0.03
        
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
            float4 _FocusPos;
            fixed _SampleNumer;
            fixed _ScaleFactor;

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

                fixed2 dir = _FocusPos.xy - i.uv.xy;

                for(float index = 1; index < _SampleNumer; ++index)
                {
                    col += tex2D(_MainTex, i.uv + dir * index * _ScaleFactor);
                }
                col = col / _SampleNumer;

                return col;
            }
            ENDCG
        }
    }
}