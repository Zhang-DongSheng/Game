Shader "Hidden/Aubergine/Scratch" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Noise ("Noise (RGB)", 2D) = "white" {}
		_Speed1 ("Speed1", Float) = 0.03
		_Speed2 ("Speed2", Float) = 0.01
		_Intensity ("Intensity", Float) = 0.50
		_ScratchWidth ("ScratchWidth", Float) = 0.01
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
				uniform sampler2D _Noise;
				uniform float _Speed1, _Speed2;
				uniform float _Intensity, _ScratchWidth;

				float4 frag (v2f_img i) : COLOR {
					float ScanLine = (_Time.y * _Speed1);
					float Side = (_Time.y * _Speed2);
					float4 col = tex2D(_MainTex, i.uv);
					float scratch = tex2D(_Noise, float2(i.uv.x+Side, ScanLine)).x;
					scratch = 2.0f*(scratch - _Intensity)/_ScratchWidth;
					scratch = 1.0f-abs(1.0-scratch);
					scratch = max(0, scratch);
					return col + float4(scratch.xxx, 0);
				}
			ENDCG
		}
	}
	Fallback off
}