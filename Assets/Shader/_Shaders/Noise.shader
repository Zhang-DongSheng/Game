Shader "Hidden/Aubergine/Noise" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_NoiseScale ("Noise Scale", Float) = 0.5
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
				float _NoiseScale;
			
				float4 frag (v2f_img i) : COLOR {
					float2 t = i.uv;
					float x = t.x * t.y * 123456 * _Time.y;
					x = fmod(x, 13) * fmod(x, 123);
					float dx = fmod(x, 0.01);
					float dy = fmod(x, 0.012);
					float4 col1 = tex2D(_MainTex, t+(float2(dx, dy) * _NoiseScale));
					float4 col2 = tex2D(_MainTex, t-(float2(dx, dy) * _NoiseScale));
					return lerp(col1, col2, 0.5);
				}
			ENDCG
		}
	}
	Fallback off
}