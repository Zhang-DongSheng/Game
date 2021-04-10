// 15.10.2 [映射, 使用GrabPass, 需要用球体显示]
Shader "Study/GrabPass/Mapping"
{
    SubShader
    {
        GrabPass
        {
            "_MyGrab"
        }
        Pass
        {
            Tags
            {
                "LightMode"="Vertex"
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MyGrab;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 vc : TEXCOORD1;
            };

            v2f vert (appdata_full v)
            {
                v2f o;

                o.pos = UnityObjectToClipPos(v.vertex);

                float3 vn = mul(UNITY_MATRIX_MV, SCALED_NORMAL);

                o.uv.x = asin(vn.x) / 3.14 + 0.5;

                o.uv.y = asin(vn.y) / 3.14 + 0.5;

                o.vc = ShadeVertexLights(v.vertex, v.normal);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MyGrab, i.uv);

                return color * float4(i.vc , 1) * 3;
            }
            ENDCG
        }
    }
}