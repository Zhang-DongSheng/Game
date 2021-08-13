Shader "Hidden/Aubergine/RadialBlur" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_CenterX ("CenterX", Float) = 0.5
		_CenterY ("CenterY", Float) = 0.5
	}
	
	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off Lighting Off Fog { Mode off }
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 
				#include "UnityCG.cginc"
				//#pragma target 3.0

				uniform sampler2D _MainTex;
				float _CenterX, _CenterY;

				float4 frag (v2f_img i) : COLOR {
					half4 c = 0;
					float2 center = float2(_CenterX, _CenterY);
					i.uv -= center;
					for(int a = 0; a<12; a++) {
						float scale = 1 + (-0.2 * a/(11.0));
						c += tex2D(_MainTex, i.uv*scale + center);
					}
					c /= 12;
					return c;
				}
			ENDCG
		}
	}
	Fallback off
}