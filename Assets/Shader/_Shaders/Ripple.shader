Shader "Hidden/Aubergine/Ripple" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Amount ("Ripple Amount", Float) = 16
		_Speed ("Ripple Speed", Float) = 4
		_Strength ("Ripple Strength", Float) = 0.009
		_OffsetX ("OffsetX", Float) = 0.0
		_OffsetY ("OffsetY", Float) = 0.0
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
				float _Amount, _Speed, _Strength, _OffsetX, _OffsetY;

				float4 frag (v2f_img i) : COLOR {
					float2 tUV = i.uv;
					float2 p = (-1.0 + 2.0 * tUV) - float2(_OffsetX, _OffsetY);
					float len = length(p);
					float2 fUV = tUV + (p/len) * cos(len*_Amount - _Time.y * _Speed) * _Strength;

					float4 col = tex2D(_MainTex, fUV);
					return col;
				}
			ENDCG
		}
	}
	Fallback off
}