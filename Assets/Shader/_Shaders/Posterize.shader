Shader "Hidden/Aubergine/Posterize" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Colors ("Colors", Float) = 4.0
		_Gamma ("Gamma", Float) = 1.0
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
				float _Colors;
				float _Gamma;

				float4 frag (v2f_img i) : COLOR {
					float4 col = tex2D(_MainTex, i.uv);
					float3 tCol = col.xyz;
					tCol = pow(tCol, _Gamma);
					tCol = tCol * _Colors;
					tCol = floor(tCol);
					tCol = tCol / _Colors;
					tCol = pow(tCol, 1.0 / _Gamma);
					return float4(tCol, col.w);
				}
			ENDCG
		}
	}
	Fallback off
}