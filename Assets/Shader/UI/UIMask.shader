// Shader: UI遮罩
Shader "UI/Mask"
{
    Properties 
    {
        [PerRendererData] _MainTex ("MainTex", 2D) = "white" {}
        _MaskTex ("MaskTex", 2D) = "white" {}
        _Horizontal ("Horizontal", Float) = 0
        _Vertical ("Vertical", Float) = 0
        _Scale ("Scale", Float) = 100
    }
    SubShader 
    {
        Tags { "RenderType"="Transparent" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Lighting Off ZWrite Off

            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _MaskTex; uniform float4 _MaskTex_ST;
            uniform float _Horizontal;
            uniform float _Vertical;
            uniform float _Scale;

            struct a2v 
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f 
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 position : TEXCOORD1;
            };

            v2f vert (a2v v)
            {
                v2f o = (v2f)0;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = v.texcoord;

                o.position = mul(unity_ObjectToWorld, v.vertex).xy;

                o.position = o.position / _Scale;
                
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET 
            {
                fixed4 color;
                color = tex2D(_MainTex, TRANSFORM_TEX(i.uv, _MainTex));
                i.position.x -= _Horizontal; i.position.y -= _Vertical;
                if (i.position.x >= 0 && i.position.x <= 1 &&
                    i.position.y >= 0 && i.position.y <= 1)
                {
                    fixed4 mask = tex2D(_MaskTex, TRANSFORM_TEX(i.position, _MaskTex));
                    if (mask.a > 0)
                        color.a = 0;
                }
                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}