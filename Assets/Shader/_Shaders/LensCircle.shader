Shader "Hidden/Aubergine/LensCircle" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_RadiusX ("RadiusX", Float) = 1.0
		_RadiusY ("RadiusY", Float) = 0.0
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
				uniform float _RadiusX;
				uniform float _RadiusY;

				float4 frag (v2f_img i) : COLOR {
					half4 col = tex2D(_MainTex, i.uv);
					float dist = distance(i.uv, float2(0.5, 0.5));
					col.rgb *= smoothstep(_RadiusX, _RadiusY, dist);
					return col;
				}
			ENDCG
		}
	}
	Fallback off
}