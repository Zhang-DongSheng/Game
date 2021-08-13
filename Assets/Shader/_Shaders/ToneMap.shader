Shader "Hidden/Aubergine/ToneMap" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Exposure ("Exposure", Float) = 0.1
		_Gamma ("Gamma", Float) = 1.0
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
				uniform float _Exposure;
				uniform float _Gamma;

				float4 frag (v2f_img i) : COLOR {
					half3 col = tex2D(_MainTex, i.uv).rgb;
					
					col *= pow(2.0, _Exposure);
					col = pow(col, _Gamma/2.2);
					return half4(col.rgb, 1.0);
				}
			ENDCG
		}
	}
	Fallback off
}