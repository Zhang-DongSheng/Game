Shader "Hidden/Aubergine/Pulse" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Speed ("Speed", Float) = 5.0
		_Distance ("Distance", Float) = 0.1
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
				uniform float _Speed;
				uniform float _Distance;

				float4 frag (v2f_img i) : COLOR {
					float2 coord = i.uv;
					coord -= 0.5;
					coord *= 1-(sin(_Time.y*_Speed)*_Distance+_Distance)*0.5;
					coord += 0.5;
					half4 col = tex2D(_MainTex, coord);
					return col;
				}
			ENDCG
		}
	}
	Fallback off
}