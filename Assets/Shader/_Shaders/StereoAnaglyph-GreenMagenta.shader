// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Aubergine/StereoAnaglyph_GreenMagenta" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_TexSizeX ("TexSizeX", Float) = 800
		_TexSizeY ("TexSizeY", Float) = 600
		_Distance ("Distance", Float) = 0.005
	}

	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off Lighting Off Fog { Mode off }
			
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"

				uniform sampler2D _MainTex;
				uniform float _TexSizeX = 800;
				uniform float _TexSizeY = 600;
				uniform float _Distance = 0.1;
				
				struct v2f {
					float4 pos : SV_POSITION;
					half2 uv : TEXCOORD0;
				};
				
				v2f vert( appdata_img v ) {
					v2f o;
					o.pos = UnityObjectToClipPos (v.vertex);
					float2 alignment = float2(0.5/_TexSizeX, 0.5/_TexSizeY);
					o.uv = MultiplyUV( UNITY_MATRIX_TEXTURE0, v.texcoord + alignment);
					return o;
				}

				float4 frag (v2f i) : COLOR {
					// dubois green magenta matrix 1
					float3x3 m0 = float3x3(
									-0.062,  0.284, -0.015,
									-0.158,  0.668, -0.027,
									-0.039,  0.143,  0.021);
					// dubois green magenta matrix 2
					float3x3 m1 = float3x3(
									0.529, -0.016,  0.009,
									0.705, -0.015,  0.075,
									0.024, -0.065,  0.937);
					float2 left = i.uv; 
					float2 right = i.uv;
					if (i.uv.x < 0.5f) {
						left = i.uv;
						right = i.uv + float2(_Distance, 0.0f);
					}
					if (i.uv.x >= 0.5f) {
						right = i.uv;
						left = i.uv - float2(_Distance, 0.0f);
					}
					float4 leftCol = tex2D(_MainTex, left);
					//Gamma fix
					leftCol = pow(leftCol, 0.9/2.2);
					float4 rightCol = tex2D(_MainTex, right);
					//Gamma fix
					rightCol = pow(rightCol, 1.25/2.2);
					
					float3 col = mul(leftCol.xyz, m0) + mul(rightCol.xyz, m1);
					return float4(col, 1);
				}
			ENDCG
		}
	}
	Fallback off
}