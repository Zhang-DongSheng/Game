Shader "Hidden/Aubergine/Desaturate" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Amount ("_Amount", Float) = 0.5
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
				float _Amount;

				float4 frag (v2f_img i) : COLOR {
					float3 col = tex2D(_MainTex, i.uv).xyz;
					float3 lum = float3(0.3, 0.59, 0.11);
					float3 gray = dot(lum, col).xxx;
					float3 result = lerp(col, gray, _Amount);
					return float4(result, 1);
				}
			ENDCG
		}
	}
	Fallback off
}