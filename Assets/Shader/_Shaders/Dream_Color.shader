Shader "Hidden/Aubergine/Dream_Color" {
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

				uniform sampler2D _MainTex;

				float4 frag (v2f_img i) : COLOR {
					float4 col = tex2D(_MainTex, i.uv);
					col += tex2D(_MainTex, i.uv+0.001);
					col += tex2D(_MainTex, i.uv+0.003);
					col += tex2D(_MainTex, i.uv+0.005);
					col += tex2D(_MainTex, i.uv+0.007);
					col += tex2D(_MainTex, i.uv+0.009);
					col += tex2D(_MainTex, i.uv+0.011);
					
					col += tex2D(_MainTex, i.uv-0.001);
					col += tex2D(_MainTex, i.uv-0.003);
					col += tex2D(_MainTex, i.uv-0.005);
					col += tex2D(_MainTex, i.uv-0.007);
					col += tex2D(_MainTex, i.uv-0.009);
					col /= 9.5;
					return col;
				}
			ENDCG
		}
	}
	Fallback off
}