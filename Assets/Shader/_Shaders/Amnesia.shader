Shader "Hidden/Aubergine/Amnesia" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Density ("Amplitude", Float) = 1.0
		_Speed ("Speed", Float) = 3.0
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
				float _Density, _Speed;
				
				float4 frag (v2f_img i) : COLOR {
					float4 col = tex2D(_MainTex, i.uv);
					col *= 0.75 + 0.25*cos((i.uv.y/_ScreenParams.y - _Time.y * _Speed) * _Density);
					return col;
				}
			ENDCG
		}
	}
	Fallback off
}