Shader "Hidden/Aubergine/Spherical" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Amount ("Ripple Amount", Float) = 16
		_Speed ("Ripple Speed", Float) = 4
		_Strength ("Ripple Strength", Float) = 0.009
	}
	
	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off Lighting Off Fog { Mode off }
			CGPROGRAM
				//if you want to use this shader in gles,
				//you must remove the "if (r > 1.0) discard;" line from fragment
				#pragma exclude_renderers gles
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 
				#include "UnityCG.cginc"

				uniform sampler2D _MainTex;
				float _Amount, _Speed, _Strength;

				float4 frag (v2f_img i) : COLOR {
					float2 p = -1 + 2 * i.uv;
					float2 fUV;
					float r = dot(p, p);
					//delete below line if you want to show the whole scene
					if (r > 1.0) discard;
					//Below f is for showing inside a sphere
					float f = (sqrt(1-r*r));
					//Below f is for showing outisde a sphere, (only use 1 f value) 
					//float f = (1-sqrt(1-r))/r;
					fUV.x = p.x*f/2 + .5;
					fUV.y = p.y*f + .5;

					float4 col = tex2D(_MainTex, fUV);
					return col;
				}
			ENDCG
		}
	}
	Fallback off
}