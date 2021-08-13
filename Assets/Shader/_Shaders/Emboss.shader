Shader "Hidden/Aubergine/Emboss" {
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
					half4 col = tex2D(_MainTex, i.uv);
					col -= tex2D(_MainTex, i.uv+0.001)*2.0;
					col += tex2D(_MainTex, i.uv-0.001)*2.0;
					col.rgb = (col.r+col.g+col.b)/3.0;
					return col;
				}
			ENDCG
		}
	}
	Fallback off
}