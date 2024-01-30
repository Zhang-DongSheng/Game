Shader "UI/Hole"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _Area ("Area", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Blend SrcAlpha OneMinusSrcAlpha
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 position : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Area;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.position = mul(unity_ObjectToWorld, v.vertex).xy;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, i.uv);

                if (i.position.x > _Area.x - _Area.z * 0.5 &&
                    i.position.x < _Area.x + _Area.z * 0.5 &&
                    i.position.y > _Area.y - _Area.w * 0.5 &&
                    i.position.y < _Area.y + _Area.w * 0.5)
                {
                    color = fixed4(0, 0, 0, 0);
                }
                return color;
            }
            ENDCG
        }
    }
}
