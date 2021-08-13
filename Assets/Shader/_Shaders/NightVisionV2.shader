Shader "Hidden/Aubergine/NightVisionV2" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_NoiseTex ("Noise Map", 2D) = "white" {}
		_NoiseAmount ("Noise Amount", float) = 0.9
		_LumThreshold ("LumThreshold", float) = 0.2
		_BrightenFactor ("BrightenFactor", float) = 2.0
		_VisionColor ("Vision Color", Color)  = (0.1, 0.95, 0.2, 1)
	}
	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off Lighting Off Fog { Mode off }
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"

				sampler2D _MainTex, _NoiseTex;
				float _NoiseAmount, _LumThreshold, _BrightenFactor;
				float4 _VisionColor;

				float4 frag (v2f_img i) : COLOR {
					float4 col = tex2D(_MainTex, i.uv);
					float3 res = 0;
					float dist = length(i.uv - float2(0.5, 0.5));
					float lum = dot(col.rgb, float3(0.3, 0.59, 0.11));
					if (lum < _LumThreshold) col.rgb *= _BrightenFactor;
					res.rgb = saturate(col.rgb * _VisionColor.xyz + tex2D(_NoiseTex, _NoiseAmount * (i.uv + sin(_Time.y * 50.0))).xyz );
					return float4(res, col.a);
				}
			ENDCG
		}
	}
	Fallback off
}