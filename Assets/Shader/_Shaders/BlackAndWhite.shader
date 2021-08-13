Shader "Hidden/Aubergine/BlackAndWhite" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Threshold ("Threshold", float) = 0.5
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
				float _Threshold;

				half4 frag (v2f_img i) : COLOR {
					half4 col = tex2D(_MainTex, i.uv);
					col.rgb = (col.r + col.g + col.b) / 3.0;
					if (col.r < _Threshold) col.r = 0.0;
					else col.r = 1.0;
					return half4(col.rrr, col.a);
				}
			ENDCG
		}
	}
	Fallback off
}