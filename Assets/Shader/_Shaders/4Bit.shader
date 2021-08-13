Shader "Hidden/Aubergine/4Bit" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off Lighting Off Fog { Mode off }
		Pass {
			//Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 
				#include "UnityCG.cginc"

				uniform sampler2D _MainTex;
				uniform float _BitDepth;
				uniform float _Contrast;
				
				float4 frag (v2f_img i) : COLOR {
					float4 col = tex2D(_MainTex, i.uv);
					col.x = 0.5 + _Contrast * (col.x - 0.5);
					col.y = 0.5 + _Contrast * (col.y - 0.5);
					col.z = 0.5 + _Contrast * (col.z - 0.5);
					float K = 256;
					if ((int)_BitDepth <= 8) K = 255;
					if ((int)_BitDepth <= 7) K = 128;
					if ((int)_BitDepth <= 6) K = 64;
					if ((int)_BitDepth <= 5) K = 32;
					if ((int)_BitDepth <= 4) K = 16;
					if ((int)_BitDepth <= 3) K = 8;
					if ((int)_BitDepth <= 2) K = 4;
					if ((int)_BitDepth <= 1) K = 2;
					
					int r = (col.x + 1.0/K) * K;
					int g = (col.y + 1.0/K) * K;
					int b = (col.z + 1.0/K) * K;
					
					float4 res = float4(r/K, g/K, b/K, col.a);
					return res;
				}
			ENDCG
		}
	}
	Fallback off
}