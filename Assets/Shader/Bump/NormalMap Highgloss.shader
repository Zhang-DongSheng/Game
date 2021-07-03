Shader "Bump/Highgloss Normal Map"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _MainTex ("Base 2D", 2D) = "white" {}
        _BumpMap ("Bump Map", 2D) = "white" {}
        _BumpScale ("Bump Scale", Range(0.1, 3)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            #pragma vertex vert
            #pragma fragment frag

            fixed4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _BumpMap;
            float4 _BumpMap_ST;
            float _BumpScale;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                float3 lightDir : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
            };

            v2f vert (appdata_full v)
            {
                v2f o;

                o.pos = UnityObjectToClipPos(v.vertex);

                o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;

				o.uv.zw = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;

                TANGENT_SPACE_ROTATION;

                o.lightDir = ObjSpaceLightDir(v.vertex);

                o.lightDir = mul(rotation, o.lightDir);

                o.viewDir = ObjSpaceViewDir(v.vertex);

                o.viewDir = mul(rotation, o.viewDir);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 main = tex2D(_MainTex, i.uv.xy) * _Color;

                fixed4 bump = tex2D(_BumpMap, i.uv.zw);

                float3 normal = UnpackNormal(bump);

                normal.xy = normal.xy * _BumpScale;

				normal = normalize(normal);

                float3 dir = normalize(i.lightDir + i.viewDir);

                float hight = max(0, dot(normal, dir));

                fixed3 diffuse = _LightColor0 * max(0, dot(normal, i.lightDir)) * main;

                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * main;

                float3 spec = pow(hight, 32.0) * main;

				fixed3 color = diffuse + ambient + spec;
                
                return fixed4(color, 1);
            }
            ENDCG
        }
    }
}