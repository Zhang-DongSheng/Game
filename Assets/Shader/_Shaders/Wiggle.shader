Shader "Hidden/Aubergine/Wiggle" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Speed ("Speed", Float) = 10.0
		_Amplitude ("Amplitude", Float) = 0.01
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
					i.uv.x += sin(_Time.y + i.uv.x * _Speed) * _Amplitude;
					i.uv.y += cos(_Time.y + i.uv.y * _Speed) * _Amplitude;
					float4 col = tex2D(_MainTex, i.uv);
					return col;
				}
			ENDCG
		}
	}
	Fallback off
}