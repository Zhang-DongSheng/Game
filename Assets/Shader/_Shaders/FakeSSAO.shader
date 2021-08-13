Shader "Hidden/Aubergine/FakeSSAO" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BaseC ("Base", Float) = 4
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
				uniform int _BaseC;

				float4 frag (v2f_img i) : COLOR {
					float4 col;
					float lum, dev, curve;
					col = tex2D(_MainTex, i.uv);
					
					lum = (col.r + col.g + col.b) / 3;
					lum = 4-(lum*(1/pow(lum, 2)));
					lum = smoothstep(_BaseC * -1, 3, lum);
					curve = (0.75 * pow(lum, 3)) + 0.25;
					curve += 0.075;
					col = col * curve;
					//return curve;
					return col;
				}
			ENDCG
		}
	}
	Fallback off
}