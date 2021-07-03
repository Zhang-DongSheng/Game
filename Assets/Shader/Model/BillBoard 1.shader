Shader "Model/BillBoard 1"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
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
            float4 _MainTex_ST;

            v2f vert (appdata_base v)
            {
                v2f o;

                float4 ori = mul(UNITY_MATRIX_MV, float4(0,0,0,1));

                float4 vertex = v.vertex;

                vertex.y = vertex.z;

                vertex.z = 0;

                vertex.xyz += ori.xyz;

                o.pos = mul(UNITY_MATRIX_P, vertex);

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