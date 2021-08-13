Shader "Hidden/Aubergine/Godrays1" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	
	/*
	PLEASE PAY ATTENTION TO REMARKS BELOW
	FOR THE SAKE OF SPEED, THIS SHADER HAS TO BE HAND EDITED BY YOU
	*/

	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off Lighting Off Fog { Mode off }
			
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				//#pragma target 3.0
				#include "UnityCG.cginc"

				uniform sampler2D _MainTex;

				float4 frag (v2f_img i) : COLOR {
					float4 blurred = 0;
					float2 myUv = i.uv - float2(0.5f, 0.5f);
					//PAY ATTENTION BELOW
					//12 is the amount of samples, this is the limit for shader 2.0
					//if you want to target 3.0 and want a smoother effect,
					//you can go up to 96
					for(int a=0; a<12; a++) {
						//PAY ATTENTION BELOW
						//(-0.5) is the distance of blurred image spacing
						//the more it is, less dense the effect is
						//a/(11) is AMOUNTOFSAMPLES-1 so, change that value according
						//to as well
						//if you set samples to 24, this value has to be 23
						float scale = 1 + (-0.5)*((float)a/(11));
						blurred += tex2D(_MainTex, (myUv * scale) + float2(0.5f, 0.5f));
					}
					//PAY ATTENTION BELOW
					//if you change the above amount of samples, this should be the same
					//as above
					blurred /= 12; // <-------THIS ONE
					blurred.rgb = pow(blurred.rgb, 2.2);
					blurred.rgb *= 3.0;
					blurred.rgb = saturate(blurred.rgb);
					
					half4 screen = tex2D(_MainTex, myUv + float2(0.5f, 0.5f));
					half3 col = screen.rgb + blurred.rgb;
					half alpha = max(screen.a, 0);
					
					return half4(col, alpha);
				}
			ENDCG
		}
	}
	Fallback off
}