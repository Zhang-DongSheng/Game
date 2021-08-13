Shader "Hidden/Aubergine/SinCity" {
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
					float4 grays;
					float gray;
					gray = 1.5*(col.r + col.g + col.b)/3.0;
					grays.r = gray;
					grays.g = gray;
					grays.b = gray;
					grays.a = 1.0;
					if (col.r > col.g*1.2 && col.r > col.b*2.0) {
						grays.g = col.g*0.25;
						grays.b = col.b*0.25;
						grays.r = col.r*1.25;
					}
					return grays;
				}
			ENDCG
		}
	}
	Fallback off
}