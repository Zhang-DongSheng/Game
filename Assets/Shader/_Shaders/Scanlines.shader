Shader "Hidden/Aubergine/Scanlines" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Intense1 ("Intense1", Float) = 0.9
		_Intense2 ("Intense2", Float) = 0.5
		_Count ("Count", Float) = 15.0
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
				uniform float _Intense1;
				uniform float _Intense2;
				uniform float _Count;

				float4 frag (v2f_img i) : COLOR {
					float4 col = tex2D(_MainTex, i.uv);
					float n = i.uv.y  - _Time.y * 1000;
					n = fmod(n, 13) * fmod(n, 123);
					float dn = fmod(n, 0.01);
					float3 res = col.rgb + col.rgb * saturate(0.1f + dn.xxx * 100);
					float2 sc;
					sincos(i.uv.y * _Count, sc.x, sc.y);
					res += col.rgb  * _Intense1;
					res = lerp (col, res, saturate(_Intense2));
					res.rgb = float3((res.r+res.g+res.b)/3.0);
					return float4(res, col.a);
				}
			ENDCG
		}
	}
	Fallback off
}