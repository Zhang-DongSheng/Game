Shader "Hidden/Aubergine/Frost" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_NoiseMap ("NoiseMap", 2D) = "white" {}
		_Amount ("Amount", Float) = 1.0
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
				sampler2D _NoiseMap;
				float _Amount;

				float4 frag (v2f_img i) : COLOR {
					float2 ox = float2(0.005, 0.0);
					float2 oy = float2(0.0, 0.005);
					float4 c1 = tex2D(_MainTex, i.uv - oy - ox);
					float4 c2 = tex2D(_MainTex, i.uv + oy + ox);
					float n = tex2D(_NoiseMap, i.uv * _Amount);
					n = fmod(n, 0.111111)/0.111111;
					float4 result = lerp(c1, c2, n);
					return result;
				}
			ENDCG
		}
	}
	Fallback off
}