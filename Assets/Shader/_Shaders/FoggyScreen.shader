Shader "Hidden/Aubergine/FoggyScreen" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_FogColor ("Fog Color", Color)  = (1.0, 0.5, 0.5, 1.0)
		_FogThickness ("Fog Thickness", float) = 1
	}
	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off Lighting Off Fog { Mode off }
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"
				
				sampler2D _MainTex;
				sampler2D _CameraDepthTexture;
				float4 _MainTex_TexelSize;
				float4 _FogColor;
				float _FogThickness;
				
				float4 frag (v2f_img i) : COLOR {
					float4 tex = tex2D(_MainTex, i.uv);
					float depth = length(tex2D(_CameraDepthTexture, i.uv).xyz);
					
					return min(_FogColor, lerp(tex, _FogColor, 1.0f - (1.0f / pow(2.2, depth * _FogThickness))));
				}
			ENDCG
		}
	}
	Fallback off
}