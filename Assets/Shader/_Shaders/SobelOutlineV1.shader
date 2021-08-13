Shader "Hidden/Aubergine/SobelOutlineV1" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Threshold ("Threshold", Float) = 0.7
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
				float _Threshold;
				
				half4 frag (v2f_img i) : COLOR {
					//Sobel
					float off = _MainTex_TexelSize;
					half4 s00 = tex2D(_MainTex, i.uv + float2(-off, -off));
					half4 s01 = tex2D(_MainTex, i.uv + float2(0, -off));
					half4 s02 = tex2D(_MainTex, i.uv + float2(off, -off));
					half4 s10 = tex2D(_MainTex, i.uv + float2(-off, 0));
					half4 s12 = tex2D(_MainTex, i.uv + float2(off, 0));
					half4 s20 = tex2D(_MainTex, i.uv + float2(-off, off));
					half4 s21 = tex2D(_MainTex, i.uv + float2(0, off));
					half4 s22 = tex2D(_MainTex, i.uv + float2(off, off));
					half4 sX = s00 + 2 * s10 + s20 - s02 - 2 * s12 - s22;
					half4 sY = s00 + 2 * s01 + s02 - s20 - 2 * s21 - s22;
					half4 eSqr = sX * sX + sY * sY;

					return (1.0 - dot(eSqr, _Threshold)) + tex2D(_MainTex, i.uv);
				}
			ENDCG
		}
	}
	Fallback off
}