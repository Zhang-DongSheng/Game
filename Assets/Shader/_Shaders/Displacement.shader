Shader "Hidden/Aubergine/Displacement" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpTex ("BumpTex", 2D) = "bump" {}
		_Amount ("Amount", Float) = 0.7
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
				uniform sampler2D _BumpTex;
				uniform float _Amount;

				float4 frag (v2f_img i) : COLOR {
					float2 bump = (tex2D(_BumpTex, i.uv).xy - 0.5) * _Amount;
					half4 col = tex2D(_MainTex, i.uv + bump);
					return col;
				}
			ENDCG
		}
	}
	Fallback off
}