Shader "Hidden/Aubergine/Pencil" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_PencilTex ("PencilTex", 2D) = "gray" {}
		_Amount ("Amount", Float) = 0.7
		_Brightness ("Brightness", Float) = 0.7
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
				uniform sampler2D _PencilTex;
				uniform float _Amount, _Brightness;

				float4 frag (v2f_img i) : COLOR {
					half4 col0 = half4(1.0, 0.0, 0.0, 0.0);
					half4 col1 = half4(0.0, 0.0, 1.0, 0.0);
					
					//Noise
					float2 nCoord;
					nCoord.x = 0.4 * sin(_Time.y * 50.0);
					nCoord.x = 0.4 * cos(_Time.y * 50.0);
					
					half4 col = tex2D(_MainTex, i.uv) * _Brightness;
					half4 pen0 = tex2D(_PencilTex, (i.uv * _Amount) + nCoord);
					half4 col2 = (1.0 - col) * pen0;
					
					half pen1 = dot(col2, col0);
					half pen2 = dot(col2, col1);
					
					half4 result = half4(pen2, pen2, pen2, pen1);
					result = (1.0 - result) * (1.0-result.a);
					result = saturate((result - 0.5) * 2.0 * col) * half4(1.0, 0.9, 0.8, 1.0);
					return result;
				}
			ENDCG
		}
	}
	Fallback off
}