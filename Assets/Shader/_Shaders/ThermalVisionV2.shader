Shader "Hidden/Aubergine/ThermalVisionV2" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ThermalTex ("Thermal Map", 2D) = "white" {}
		_NoiseTex ("Noise Map", 2D) = "white" {}
		_NoiseAmount ("Noise Amount", float) = 0.3
	}

	SubShader {
		ZTest Always Cull Off ZWrite Off Lighting Off Fog { Mode off }

		CGINCLUDE
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			sampler2D _MainTex, _ThermalTex, _NoiseTex;
			sampler2D _CameraDepthTexture;
			sampler2D _CameraDepthNormalsTexture;
			float4 _MainTex_TexelSize;
			float _NoiseAmount;
			uniform float4x4 _ViewProjectInverse;
			
			//Returns World Position
			float4 SamplePositionMap (float2 uvCoord) {
				float4 encoded = tex2D(_CameraDepthNormalsTexture, uvCoord);
				float depth = DecodeFloatRG(encoded.zw);
				// H is the viewport position at this pixel in the range -1 to 1.
				float4 H = float4(uvCoord.x * 2 - 1, (uvCoord.y) * 2 - 1, depth, 1);
				float4 D = mul(_ViewProjectInverse, H);
				return D / D.w;
			}

			//Returns Scene Normals
			float3 SampleSceneNormals (float2 uvCoord) {
				float4 encoded = tex2D(_CameraDepthNormalsTexture, uvCoord);
				return DecodeViewNormalStereo(encoded);
			}
		ENDCG
		
		Pass {
			CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				
				float4 frag (v2f_img i) : COLOR {
					float3 normal = SampleSceneNormals(i.uv);
					float3 scene = dot(tex2D(_MainTex, i.uv).rgb, float3(0.3f, 0.59f, 0.11f));
					float3 position = SamplePositionMap(i.uv).xyz;
					float3 eyeDir = float3(position - _WorldSpaceCameraPos);
					float radiation = 2.2f; //Start with default gamma
					if(length(normal.xyz) > 0.1f)
						radiation += length(dot(normalize(eyeDir), normalize(normal)));

					float3 result = 0;

					float v = scene.r * saturate(radiation);

					result.xyz = tex2D(_ThermalTex, float2(v, v) + tex2D(_NoiseTex, _NoiseAmount * (i.uv + sin(_Time.y * 50.0))).xyz);
					
					return float4(result, 1);
				}
			ENDCG
		}
	}
	Fallback off
}