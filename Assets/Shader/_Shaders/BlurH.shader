Shader "Hidden/Aubergine/BlurH" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BlurMulti ("Blur Multi", float) = 1
	}

	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off Lighting Off Fog { Mode off }
			
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"
				
				sampler2D _MainTex;
				float4 _MainTex_TexelSize;
				float _BlurMulti;
				
				static const float2 PixelKernel[11] =
				{
					{ -5, 0 },
					{ -4, 0 },
					{ -3, 0 },
					{ -2, 0 },
					{ -1, 0 },
					{  0, 0 },
					{  1, 0 },
					{  2, 0 },
					{  3, 0 },
					{  4, 0 },
					{  5, 0 },
				};

				static const float BlurWeights[11] = 
				{
					0.008764,
					0.026995,
					0.064759,
					0.120985,
					0.176033,
					0.199471,
					0.176033,
					0.120985,
					0.064759,
					0.026995,
					0.008764,
				};
				
				float4 frag (v2f_img i) : COLOR {

					float4 col = 0;
					for(int a = 0; a < 11; a++)
						col += tex2D(_MainTex, i.uv + PixelKernel[a].xy * _BlurMulti / _ScreenParams.x) * BlurWeights[a];
					return col;
				}
			ENDCG
		}
	}
	Fallback off
}