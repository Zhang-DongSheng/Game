Shader "Hidden/Aubergine/ThermalVision" {
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
				float4 thermal;

				float4 frag (v2f_img i) : COLOR {
					float4 pixcol = tex2D(_MainTex, i.uv);
					float4 colors[3];
					colors[0] = float4(0.0,0.0,1.0,1.0);
					colors[1] = float4(1.0,1.0,0.0,1.0);
					colors[2] = float4(1.0,0.0,0.0,1.0);
					float lum = (pixcol.r+pixcol.g+pixcol.b)/3.;
					int ix = (lum < 0.5)? 0:1;
					if (ix == 0) {
						thermal = colors[0] * (lum-float(ix)*0.5)/0.5 + (1 - (lum-float(ix)*0.5)/0.5) * colors[1];
					}
					if (ix == 1) {
						thermal = colors[1] * (lum-float(ix)*0.5)/0.5 + (1 - (lum-float(ix)*0.5)/0.5) * colors[2];
					}
					return thermal;
				}
			ENDCG
		}
	}
	Fallback off
}