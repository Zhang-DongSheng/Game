 Shader "Hidden/Aubergine/EdgeDetect" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
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
				uniform float4 _MainTex_TexelSize;

				float4 frag (v2f_img i) : COLOR {
					float2 sampleOffsets[8] = {
						-0.00326212, -0.00405805,
						-0.00840144, -0.00073580,
						-0.00695914,  0.00457137,
						-0.00203345,  0.00620716,
						0.00962340, -0.00194983,
						0.00473434, -0.00480026,
						0.00519456,  0.00767022,
						0.00185461, -0.00893124,
					};
					float dx = normalize(_MainTex_TexelSize.x);
					float dy = normalize(_MainTex_TexelSize.y);
					float pKernel[4] = {-dx, dx, -dy, dy};
					float qKernel[4] = {-dx, -dx, dy, dy};
					float PI = 3.1415926535897932384626433832795;
				
					float2 texCoords[4];
					texCoords[0] = i.uv + sampleOffsets[1];
					texCoords[1] = i.uv + sampleOffsets[2];
					texCoords[2] = i.uv + sampleOffsets[3];
					texCoords[3] = i.uv + sampleOffsets[4];
					
					float3 texSamples[4];
					float p=0,q=0;
					for(int i=0; i <4; i++)
					{
						texSamples[i].xyz = tex2D( _MainTex, texCoords[i]);
						texSamples[i] = dot(texSamples[i], 0.33333333f);
						p += texSamples[i] * pKernel[i];
						q += texSamples[i] * qKernel[i];
					}
					p /= 0.2;
					q /= 0.2;
					float Magnitude = sqrt((p*p) + (q*q));
					float4 result = Magnitude;
					float Theta = atan2(q,p);
					result.a = (abs(Theta) / PI);
					return result;
				}
			ENDCG
		}
	}
	Fallback off
}