Shader "Model/Billboard 2"
{
	Properties 
	{
		_MainTex ("Main Tex", 2D) = "white" {}
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		_VerticalBillboarding ("Vertical Restraints", Range(-5, 5)) = 2
	}
	SubShader 
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "DisableBatching"="True"}
		Cull off
		Pass 
		{ 
			Tags { "LightMode"="ForwardBase" }
			
			ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
		
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "Lighting.cginc"
			
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			fixed _VerticalBillboarding;
			
			struct a2v 
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};
			
			struct v2f 
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			
			v2f vert (a2v v) 
			{
				v2f o;

				float3 center = float3(0, 0, 0);

				float3 viewer = mul(unity_WorldToObject,float4(_WorldSpaceCameraPos, 1));
				
				float3 normal = viewer - center;

				normal.y = normal.y * _VerticalBillboarding;

				normal = normalize(normal);

				float3 up = abs(normal.y) > 0.999 ? float3(0, 0, 1): float3(0, 1, 0);

				float3 right = normalize(cross(up, normal)) * -1;

				up = normalize(cross(normal, right));
				
				float3 offset = v.vertex.xyz - center;

				float3 local = center + right * offset.x + up * offset.y + normal * offset.z;
              
				o.pos = UnityObjectToClipPos(float4(local, 1));

				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return tex2D (_MainTex, i.uv) * _Color;
			}
			ENDCG
		}
	}
}