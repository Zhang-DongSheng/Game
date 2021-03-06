﻿Shader "Bump/NormalMap"
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
		Tags
		{
			"RenderType" = "Opaque"
			"LightMode" = "ForwardBase"
		}

		Pass
		{
			CGPROGRAM
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			#pragma vertex vert;
			#pragma fragment frag;

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BumpMap;
			float4 _BumpMap_ST;
			float _BumpScale;

			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float3 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 lightDir : TEXCOORD0;
				float3 worldVertex : TEXCOORD1;
				float4 uv : TEXCOORD2;
				
			};

			v2f vert(a2v v)
			{
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);

				o.worldVertex = mul(v.vertex, (float3x3)unity_WorldToObject).xyz;

				o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;

				o.uv.zw = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;

				TANGENT_SPACE_ROTATION;

				o.lightDir = mul(rotation, ObjSpaceLightDir(v.vertex));

				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				fixed3 main = tex2D(_MainTex, i.uv.xy) * _Color.rgb;
				
				fixed4 bump = tex2D(_BumpMap, i.uv.zw);

				fixed3 normal = UnpackNormal(bump);

				normal.xy = normal.xy * _BumpScale;

				normal = normalize(normal);

				fixed3 lightDir = normalize(i.lightDir);

				fixed3 diffuse = _LightColor0 * max(0, dot(normal, lightDir)) * main;

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * main;

				fixed3 color = diffuse + ambient;

				return fixed4(color, 1);
			}
			ENDCG
		}
	}
}