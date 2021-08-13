Shader "Hidden/Aubergine/BloomSimple" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Strength ("Bloom Strength", Float) = 0.5
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
				float _Strength;

				float4 frag (v2f_img i) : COLOR {
					float4 col = tex2D(_MainTex, i.uv);
					float4 bloom = col;
					col.rgb = pow(bloom.rgb, _Strength);
					col.rgb *= bloom;
					col.rgb += bloom;
					return col;
				}
			ENDCG
		}
	}
	Fallback off
}