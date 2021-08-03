//‘Î…˘
Shader "GrabPass/Noise"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Noise ("Noise", 2D) = "white" {}
        _Scale ("Scale", Range(0, 1)) = 1
        _Strength ("Strength", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        GrabPass
        {
            "_Grab"
        }
        Pass
        {
            Cull Off
            ZWrite Off
            Lighting Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 pos : TEXCOORD1;
            };

            sampler2D _Grab;
            sampler2D _Noise;
            float4 _Noise_ST;
            float _Scale;
            float _Strength;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.texcoord, _Noise);

                o.pos = ComputeGrabScreenPos(o.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 offset = tex2D(_Noise, i.uv - _Time.xy * _Scale);

                i.pos.xy += offset.xy * _Strength;

                fixed4 color = tex2Dproj(_Grab, i.pos);

                color.a = 1 - _Strength;

                return color;
            }
            ENDCG
        }
    }
}