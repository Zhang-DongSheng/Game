Shader "Hidden/Aubergine/CrossHatch" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LineColor ("Line Color", Color) = (1, .1, .1, 1)
		_LineWidth ("Line Width", Float) = 0.005
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off Lighting Off Fog { Mode off }
		Pass {
			//Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 
				#include "UnityCG.cginc"

				uniform sampler2D _MainTex;
				fixed4 _LineColor;
				float _LineWidth;
				
				float4 frag (v2f_img i) : COLOR {
					float4 col = tex2D(_MainTex, i.uv);
					fixed lum = Luminance(col.rgb);
					
					fixed3 res = _LineColor.rgb;
					
					if (lum < 1.00) {
						if (fmod(i.uv.x + i.uv.y, _LineWidth * 2) < _LineWidth) { res = float3(0.0); }
					}
					if (lum < 0.75) {
						if (fmod(i.uv.x + i.uv.y, _LineWidth * 2) < _LineWidth) { res = float3(0.0); }
					}
					if (lum < 0.50) {
						if (fmod(i.uv.x + i.uv.y - _LineWidth, _LineWidth) < _LineWidth) { res = float3(0.0); }
					}
					if (lum < 0.30) {
						if (fmod(i.uv.x + i.uv.y - _LineWidth, _LineWidth) < _LineWidth) { res = float3(0.0); }
					}
					float4 fCol = float4(res, col.w);
					return fCol;
					//return col * fCol;
				}
			ENDCG
		}
	}
	Fallback off
}