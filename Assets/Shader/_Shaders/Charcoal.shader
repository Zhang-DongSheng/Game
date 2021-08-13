Shader "Hidden/Aubergine/Charcoal" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LineColor ("Line Color", Color) = (0, 0, 0, 1)
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off Lighting Off Fog { Mode off }
		Pass {
			//Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 
				#include "UnityCG.cginc"

				uniform sampler2D _MainTex;
				fixed4 _LineColor;
				float4 _MainTex_TexelSize;
				
				float4 frag (v2f_img i) : COLOR {
					float xr = _MainTex_TexelSize.x * 2;
					float yr = _MainTex_TexelSize.y * 2;
					
					float4 c1 = tex2D(_MainTex, i.uv);
					float4 c2 = tex2D(_MainTex, i.uv + float2(xr, 0));
					float4 c3 = tex2D(_MainTex, i.uv + float2(0, yr));
					
					float f = 0;
					f+= abs(c1.x-c2.x);
					f+= abs(c1.y-c2.y);
					f+= abs(c1.z-c2.z);
					
					f+= abs(c1.x-c3.x);
					f+= abs(c1.y-c3.y);
					f+= abs(c1.z-c3.z);
					
					f-= 0.2;
					f=saturate(f);
					
					c1.xyz = (1-f) + _LineColor.rgb * f;
					return c1;
				}
			ENDCG
		}
	}
	Fallback off
}