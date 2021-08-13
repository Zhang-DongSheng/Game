Shader "Hidden/Aubergine/HDR" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Amount ("Amount", Float) = 0.045
		_Multiplier ("Multiplier", Float) = 1
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
				float _Amount, _Multiplier;

				float4 frag (v2f_img i) : COLOR {
					float4 col, tCol;
					col = tCol = tex2D(_MainTex, i.uv);
					float lumT = (col.r * 0.3) + (col.g * 0.6) + (col.b * 0.1);
					float lumB = _Amount;
					//lumB = clamp(lumB, 0.001, 0.999);
					
					float std = min(lumB, (1-lumB));
					float score = (lumT - lumB) / std;
					col = (col * exp2(score)) - col;
					col *= _Multiplier;
					return col + tCol;
				}
			ENDCG
		}
	}
	Fallback off
}