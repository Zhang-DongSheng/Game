Shader "Hidden/Aubergine/Vignette" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Radius ("Radius", Float) = 3.0
		_Darkness ("Darkness", Float) = 0.5
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
				uniform float _Radius;
				uniform float _Darkness;

				float4 frag (v2f_img i) : COLOR {
					half4 col = tex2D(_MainTex, i.uv);
					float2 uv = i.uv - 0.5;
					float vignette = 1 - dot(uv, uv);
					col.rgb *= saturate(pow(vignette, _Radius) + _Darkness);
					return col;
				}
			ENDCG
		}
	}
	Fallback off
}