Shader "Hidden/Aubergine/LightWave" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Red ("Red", Float) = 4.0
		_Green ("Green", Float) = 4.0
		_Blue ("Blue", Float) = 4.0
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
				uniform float _Red, _Green, _Blue;

				half4 frag (v2f_img i) : COLOR {
					half4 col;
					col.r = tex2D(_MainTex, i.uv + float2(_MainTex_TexelSize.y * _Red, 0));
					col.g = tex2D(_MainTex, i.uv + float2(0, _MainTex_TexelSize.y * _Green));
					col.b = tex2D(_MainTex, i.uv + float2(_MainTex_TexelSize.y * _Blue, 0));
					col.w = 1;
					return col;
				}
			ENDCG
		}
	}
	Fallback off
}