Shader "Hidden/Aubergine/Negative" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
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

				float4 frag (v2f_img i) : COLOR {
					half4 col = tex2D(_MainTex, i.uv);
					return float4((float3(1,1,1)-col.xyz), col.w);
				}
			ENDCG
		}
	}
	Fallback off
}