//自定义圆角
Shader "UI/CornerPro"
{
	Properties
	{
		[PerRendererData] _MainTex ("Base(RGBA)", 2D) = "white"{}

		_Top("Top", Vector) = (-0.5, 0.5, 0.5, 0.5)

		_Bottom("Bottom", Vector) = (-0.5, -0.5, 0.5, -0.5)

		_Angle("Angle", Range(0, 180)) = 0
	}
	SubShader
	{
		Tags 
        {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

		pass
		{
			Cull Off Lighting Off ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#include "unitycg.cginc"

			#pragma vertex vert
			#pragma fragment frag
			
			sampler2D _MainTex;
			float4 _Top;
			float4 _Bottom;
			float _Angle;

			struct v2f
			{
				float4 pos : SV_POSITION ;
				float2 uv: TEXCOORD0;
				float2 center : TEXCOORD1;
			};

			v2f vert(appdata_base v )
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				o.center = v.texcoord - float2(0.5, 0.5);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_TARGET
			{
				fixed4 color = tex2D(_MainTex, i.uv);

				float2 position = float2(abs(i.center.x), abs(i.center.y));

				float2 reference;

				float radius = 0;

				bool ignore = false;

				if(i.center.y > 0)
				{
					if(i.center.x < 0)
					{
						if(i.center.x < _Top.x && i.center.y > _Top.y)
						{
							reference = abs(float2(_Top.x, _Top.y));
						}
						else
						{
							ignore = true;
						}
					}
					else
					{
						if(i.center.x > _Top.z && i.center.y > _Top.w)
						{
							reference = abs(float2(_Top.w, _Top.z));
						}
						else
						{
							ignore = true;
						}
					}
				}
				else
				{
					if(i.center.x < 0)
					{
						if(i.center.x < _Bottom.x && i.center.y < _Bottom.y)
						{
							reference = abs(float2(_Bottom.x, _Bottom.y));
						}
						else
						{
							ignore = true;
						}
					}
					else
					{
						if(i.center.x > _Bottom.z && i.center.y < _Bottom.w)
						{
							reference = abs(float2(_Bottom.w, _Bottom.z));
						}
						else
						{
							ignore = true;
						}
					}
				}

				if(!ignore)
				{
					float radian = _Angle / 180 * 3.1415926;

					position.x += position.x * cos(radian);

					position.y += position.y * sin(radian);

					float radius = max(0.5 - reference.x, 0.5 - reference.y);

					if(length(position - reference) >= radius)
					{
						color.a = 0;
					}
				}
				return color; 
			}
			ENDCG
		}
	}
}