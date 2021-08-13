Shader "Hidden/Aubergine/TilesXY" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		numTilesX ("NumTiles", Float) = 32
		numTilesY ("NumTiles", Float) = 32
		threshold ("Threshold", Float) = 0.16
		edgeColor ("EdgeColor", Color) = (0.5,0.5,0.5,1) 
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
				uniform float numTilesX, numTilesY;
				uniform float threshold;
				uniform half3 edgeColor;

				float4 frag (v2f_img i) : COLOR {
					half2 size = half2(1.0/numTilesX, 1.0/numTilesY);
					half2 pBase = i.uv - fmod(i.uv, size);
					half2 pCenter = pBase + (size/2.0);
					half2 st = (i.uv - pBase)/size;
					half4 c1 = (half4)0;
					half4 c2 = (half4)0;
					half invOff = half4((1-edgeColor),1);
					if (st.x > st.y) { c1 = invOff; }
					half thresholdB = 1.0 - threshold;
					if (st.x > thresholdB) { c2 = c1; }
					if (st.y > thresholdB) { c2 = c1; }
					half4 cBottom = c2;
					c1 = (half4)0;
					c2 = (half4)0;
					if (st.x > st.y) { c1 = invOff; }
					if (st.x < threshold) { c2 = c1; }
					if (st.y < threshold) { c2 = c1; }
					half4 cTop = c2;
					half4 tileColor = tex2D(_MainTex, pCenter);
					half4 result = tileColor + cTop - cBottom;
					return result;
				}
			ENDCG
		}
	}
	Fallback off
}