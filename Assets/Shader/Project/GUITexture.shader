Shader "Project/GUITexture"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _X ("Screen Pos X", Float) = 0
        _Y ("Screen Pos Y", Float) = 0
        _Width ("Width", Float) = 128
        _Height ("Height", Float) = 128
    }
    SubShader
    {
        Tags { "Queue"="Overlay" }
        LOD 100

        Pass
        {
            Cull Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float _X;
            float _Y;
            float _Width;
            float _Height;

            v2f vert (appdata_base v)
            {
                v2f o;

                o.pos.xy = float2(v.vertex.x - 1, v.vertex.y - 1) * 2;

                float2 scale = float2(_Width / _ScreenParams.x, _Height / _ScreenParams.y);

                float2 offset = float2(_X / _ScreenParams.x, _Y / _ScreenParams.y) * 2;

                o.pos.xy = o.pos.xy * scale + offset;
                
                o.pos.y *= _ProjectionParams.x;

                o.pos.zw = float2(0, 1);

                o.uv = v.texcoord;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}