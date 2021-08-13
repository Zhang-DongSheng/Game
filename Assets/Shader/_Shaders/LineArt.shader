Shader "Hidden/Aubergine/LineArt" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LineColor ("Line Color", Color) = (.1, .1, .1, 1)
		_LineAmount ("Line Amount", Float) = 80
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
				fixed4 _LineColor;
				float _LineAmount;
				
				float4 frag (v2f_img i) : COLOR {
					float x = i.uv.x;
					int xi = x * _LineAmount;
					float x2 = ((float)xi) / _LineAmount;
					
					float f = (x-x2) * _LineAmount;
					if (f>0.5) f=1-f;
					
					float4 r = tex2D(_MainTex, float2(x2, i.uv.y));
					float g = (r.x+r.y+r.z)/3;
					
					g*=0.5;
					
					if (f>0.45) g = 1;
					else if(f<0.5-g) g = 0;
					else {
						f=0.45-f;
						g=1-f/g;
					}
					
					r.xyz = g * _LineColor.xyz + (1-g)*(1 - _LineColor.xyz);
					r.w*= _LineColor.w * g;
					return r;
				}
			ENDCG
		}
	}
	Fallback off
}