Shader "Hidden/Aubergine/Bleach" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Opacity ("Rim Power", float) = 0.1
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
				float _Opacity;
				
				float4 frag (v2f_img i) : COLOR {
					float4 col = tex2D(_MainTex, i.uv);
					float3 lumCoef = float3(0.22, 0.707, 0.071);
					float lum = dot(lumCoef, col);
					float3 blend = lum.rrr;
					float L = min(1, max(0, 10 * (lum - 0.45)));
					float3 res1 = 2.0 * col.rgb * blend;
					float3 res2 = 1.0 - 2.0 * (1.0 - blend) * (1.0 - col.rgb);
					float3 newCol = lerp(res1, res2, L);
					float alpha = _Opacity * col.a;
					float3 mixRGB = alpha * newCol.rgb;
					mixRGB += ((1.0 - alpha) * col.rgb);
					
					return float4(mixRGB, col.a);
				}
			ENDCG
		}
	}
	Fallback off
}