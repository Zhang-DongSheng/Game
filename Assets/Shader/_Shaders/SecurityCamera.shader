Shader "Hidden/Aubergine/SecurityCamera" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Speed ("Speed", Float) = 2.0
		_Thickness ("Thickness", Float) = 3.0
		_Luminance ("Luminance", Float) = 0.5
		_Darkness ("Darkness", Float) = 0.75
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
				uniform float4 _MainTex_TexelSize;
				uniform float _Speed, _Thickness, _Luminance, _Darkness;

				half4 frag (v2f_img i) : COLOR {
					half4 col = tex2D(_MainTex, i.uv);
					col *= _Darkness + _Luminance*sin((i.uv.y/_MainTex_TexelSize.y-_Time.y * _Speed)*_Thickness);
					return col;
				}
			ENDCG
		}
	}
	Fallback off
}