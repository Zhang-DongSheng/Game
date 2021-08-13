Shader "Hidden/Aubergine/Thicken" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off Lighting Off Fog { Mode off }
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"
				
				sampler2D _MainTex;
				float4 _MainTex_TexelSize;
				const float2 RIGHT = float2(1.0, 0.0);
				const float2 LEFT = float2(-1.0, 0.0);
				const float2 DOWN = float2(0.0, 1.0);
				const float2 UP = float2(0.0, -1.0);
				
				float4 frag (v2f_img i) : COLOR {
					float3 pix[5];
					pix[0] = tex2D(_MainTex, i.uv).rgb;
					pix[1] = tex2D(_MainTex, i.uv + RIGHT * _MainTex_TexelSize).rgb;
					pix[2] = tex2D(_MainTex, i.uv + DOWN * _MainTex_TexelSize).rgb;
					pix[3] = tex2D(_MainTex, i.uv + LEFT * _MainTex_TexelSize).rgb;
					pix[4] = tex2D(_MainTex, i.uv + UP * _MainTex_TexelSize).rgb;
					float4 col = 1.0f;
					col.rgb = ((pix[0] * pix[1] * pix[2] * pix[3] * pix[4]) + pix[0]) / 2.0;
					return col;
				}
			ENDCG
		}
	}
	Fallback off
}