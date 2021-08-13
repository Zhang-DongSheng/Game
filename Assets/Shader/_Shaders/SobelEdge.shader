Shader "Hidden/Aubergine/SobelEdge" {
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

				float4 frag (v2f_img i) : COLOR {
					float col = tex2D(_MainTex, i.uv);
					float s00 = tex2D(_MainTex, i.uv + _MainTex_TexelSize * float2(-1, -1)).r;
					float s01 = tex2D(_MainTex, i.uv + _MainTex_TexelSize * float2( 0, -1)).r;
					float s02 = tex2D(_MainTex, i.uv + _MainTex_TexelSize * float2( 1, -1)).r;
					float s10 = tex2D(_MainTex, i.uv + _MainTex_TexelSize * float2(-1, 0)).r;
					float s12 = tex2D(_MainTex, i.uv + _MainTex_TexelSize * float2( 1, 0)).r;
					float s20 = tex2D(_MainTex, i.uv + _MainTex_TexelSize * float2(-1, 1)).r;
					float s21 = tex2D(_MainTex, i.uv + _MainTex_TexelSize * float2( 0, 1)).r;
					float s22 = tex2D(_MainTex, i.uv + _MainTex_TexelSize * float2( 1, 1)).r;
					
					float sobelX = s00 + 2 * s10 + s20 - s02 - 2 * s12 - s22;
					float sobelY = s00 + 2 * s01 + s02 - s20 - 2 * s21 - s22;
					
					float edgeSqr = (sobelX * sobelX + sobelY * sobelY);
					
					float4 result = edgeSqr;
					result  *= 1.0 - ((edgeSqr > _Threshold) * _Threshold);
					return result + tex2D(_MainTex, i.uv);
				}
			ENDCG
		}
	}
	Fallback off
}