Shader "Hidden/Aubergine/LightShafts" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Density ("Density", Float) = 1.0
		_Weight ("Weight", Float) = 1.0
		_Decay ("Decay", Float) = 1.0
		_Exposure ("Exposure", Float) = 1.0
		_LightSPos ("Light Screen Position", Vector) = (0,0,0,0) 

	}

	/*
	PLEASE PAY ATTENTION TO REMARKS BELOW
	FOR THE SAKE OF SPEED, THIS SHADER HAS TO BE HAND EDITED BY YOU
	*/

	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off Lighting Off Fog { Mode off }
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma target 3.0
				#include "UnityCG.cginc"

				uniform sampler2D _MainTex;
				float _Density, _Weight, _Decay, _Exposure;
				float4 _LightSPos;
				
				float4 frag (v2f_img i) : COLOR {
					half2 dUV = (i.uv - _LightSPos.xy);
					//64 is the amount of samples, the limit for shader 2.0 is 12
					//if you want to target 2.0 lower this value to 12 and delete
					//pragma target 3.0 above
					dUV *= 1.0f / 64 * _Density;
					half3 col = tex2D(_MainTex, i.uv).rgb;
					half illum = 1.0f;
					//change below 64 to lower than 12 if you want to target 2.0
					for (int a = 0; a < 64; a++) {
						i.uv -= dUV;
						half3 sample = tex2D(_MainTex, i.uv);
						sample *= illum * _Weight;
						col += sample;
						illum *= _Decay;
					}
					return float4(col * _Exposure, 1);
				}
			ENDCG
		}
	}
	Fallback off
}