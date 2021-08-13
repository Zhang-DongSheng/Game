Shader "Hidden/Aubergine/ScreenWaves" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Speed ("Speed", Float) = 10.0
		_Amplitude ("Amplitude", Float) = 0.09
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
				float _Speed;
				float _Amplitude;

				float4 frag (v2f_img i) : COLOR {
					i.uv.y += sin(_Time.y + i.uv.x * _Speed) * _Amplitude;
					float4 result = tex2D(_MainTex, i.uv);
					return result;
				}
			ENDCG
		}
	}
	Fallback off
}