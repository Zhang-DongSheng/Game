Shader "Model/Shield2"
{
    Properties
    {
		_diffuse("kuosan",float) = 0
		_noiseTex("noiseTex",2D) = "black"{}
		_gradient("gradientTex",2D) = "black"{}
		_baseColor("baseColor",Color) = (1,1,1,1)
		_noisePow("noisePow",float) = 1
		_addSize("addSize",float) = 0
		_divSize("divSize",float) = 1
		_noiseTilling("noiseTilling",Vector) = (1,1,1,1)
		_hitSpeed("hitSpeed",float) = 10

    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 100

        Pass
        {
			Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
  

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float3 normal:NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float4 vertex_world:TEXCOORD1;
				float3 normal_world:TEXCOORD2;
            };

			float3 _hitCenter[10];
			float _hitSize[10];
			float _diffuse;
			sampler2D _noiseTex;
			float4 _noiseTex_ST;
			sampler2D _gradient;
			float4 _baseColor;
			float _noisePow;
			float _addSize;
			float _divSize;
			float2 _noiseTilling;
			float _hitSpeed;

			inline float4 TriplanarSampling44(sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index)
			{
				float3 projNormal = (pow(abs(worldNormal), falloff));
				projNormal /= (projNormal.x + projNormal.y + projNormal.z) + 0.00001;
				float3 nsign = sign(worldNormal);
				half4 xNorm; half4 yNorm; half4 zNorm;
				xNorm = tex2D(topTexMap, tiling * worldPos.zy * float2(nsign.x, 1.0));
				yNorm = tex2D(topTexMap, tiling * worldPos.xz * float2(nsign.y, 1.0));
				zNorm = tex2D(topTexMap, tiling * worldPos.xy * float2(-nsign.z, 1.0));
				return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
			}
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.vertex_world = mul(v.vertex, unity_ObjectToWorld);
				o.normal_world = mul(unity_WorldToObject, v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float alpha = 0;
				for (int j = 0;j < 10;j++)
				{
					float dis = distance(_hitCenter[j], i.vertex_world);
					float hitMaskVal = (1 - (dis - _addSize)) / (max(_divSize, 0.001f));

					float hitSizeVal = (dis - (_hitSize[j] * _hitSpeed)) / _diffuse;

					//float2 noiseUV = i.uv*_noiseTex_ST.xy + _noiseTex_ST.zw;
					//float4 noiseCol = tex2D(_noiseTex, noiseUV);
					float4 noiseCol = TriplanarSampling44(_noiseTex, i.vertex_world, i.normal_world, 1.0, _noiseTilling, 1, 0);
					float noisePowerVal = pow(noiseCol.r, _noisePow);

					float hitRange = clamp(hitSizeVal - noisePowerVal, 0, 1);

					float2 jianbianUV = float2(hitRange, 0);
					float4 jianbianCol = tex2D(_gradient, jianbianUV);

					alpha = alpha + clamp(hitMaskVal* jianbianCol.r, 0, 1);
				}
				alpha *= _baseColor.a;

                return float4(_baseColor.rgb, alpha);
            }
            ENDCG
        }
    }
}
