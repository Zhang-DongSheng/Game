Shader "Hidden/Aubergine/Raindrops" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpTex ("BumpTex", 2D) = "bump" {}
		_Amount ("Amount", Float) = 0.7
		_SpeedX ("SpeedX", Float) = 0.0
		_SpeedY ("SpeedY", Float) = 0.1
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
				uniform float _SpeedX, _SpeedY;

				float4 frag (v2f_img i) : COLOR {
					float2 dispUv;
					dispUv.x = i.uv.x + _SpeedX * _Time.y;
					dispUv.y = i.uv.y + _SpeedY * _Time.y;
					float2 disp = _Amount*(tex2D(_BumpTex, dispUv).xy - 0.5);
					half4 col = tex2D(_MainTex, i.uv+disp);
					return col;
				}
			ENDCG
		}
	}
	Fallback off
}